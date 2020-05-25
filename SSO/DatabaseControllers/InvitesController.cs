using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSO.Database;
using SSO.Models;
using SSO.Models.DatabaseModels;
using SSO.Services.Abstract;

namespace SSO.DatabaseControllers
{
    [ApiController]
    public class InvitesController : ControllerBase
    {
        private readonly SSOContext _context;
        private readonly ITokenCheck tokenCheck;
        private readonly IPermissionCheck permissionCheck;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ConfigModel config = ConfigurationExtensions.GetConfig();

        public InvitesController(SSOContext context, ITokenCheck tokenCheck, IPermissionCheck permissionCheck, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            this.tokenCheck = tokenCheck;
            this.permissionCheck = permissionCheck;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route(Routes.InviteByCompany)]
        public async Task<ActionResult<IEnumerable<Invite>>> GetInvitesByCompany(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    var output = await _context.Invites
                        .Include(x => x.Company)
                        .Where(x => x.CompanyId == id)
                        .ToListAsync();

                    if (output.Any())
                        return output;
                }
            throw new ArgumentNullException();
        }

        // GET: api/Invites
        [HttpGet]
        [Route(Routes.InviteList)]
        public async Task<ActionResult<IEnumerable<Invite>>> GetInvites([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                var output = await _context.Invites
                    .Include(x => x.Company)
                    .ToListAsync();

                if (output.Any())
                    return output;
            }
            throw new ArgumentNullException();
        }

        // GET: api/Invites/5
        [HttpGet]
        [Route(Routes.InviteItem)]
        public async Task<ActionResult<Invite>> GetInvite(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    var invites = await _context.Invites
                        .Include(x => x.Company)
                        .Where(x => x.Id == id)
                        .FirstAsync();

                    if (invites != null)
                        return invites;
                }
            throw new ArgumentNullException();
        }



        // POST: api/Invites
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route(Routes.InviteCreate)]
        public async Task<ActionResult<Invite>> PostInvite([FromBody]Invite invite, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (invite != null)
                    if (permissionCheck.IsAllowed(inputToken, "root"))
                    {
                        try
                        {
                            //create query string
                            var builder = new UriBuilder(config.SignUpURL);
                            var query = HttpUtility.ParseQueryString(builder.Query);
                            string token = BCrypt.Net.BCrypt.HashPassword(invite.Email + DateTimeOffset.UtcNow.ToString(), config.Salt.Value);
                            query.Add("token", token);
                            builder.Query = query.ToString();
                            string url = builder.ToString();//link to the signup
                            invite.Token = token;
                            //send to sns
                            var data = new
                            {
                                invite.Email,
                                Subject = "Notification",
                                Text = $"This is your invitation link: {url}"
                            };
                            var client = httpClientFactory.CreateClient();
                            var requestMessage = new HttpRequestMessage(HttpMethod.Post, config.SNS.Url);
                            requestMessage.Headers.Add("Accept", "*/*");
                            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                            //requestMessage.Headers.Add("Bearer", token);
                            var response = client.SendAsync(requestMessage).Result;
                            var responseString = response.Content.ReadAsStringAsync().Result;
                            var responseStatus = response.StatusCode;
                            //var outputDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
                            //return outputDictionary.Values.First();

                            //create invite
                            if (responseStatus == HttpStatusCode.OK)
                            {
                                _context.Invites.Add(invite);
                                await _context.SaveChangesAsync();

                                _context.Entry(invite).State = EntityState.Detached;

                                return await _context.Invites
                                    .Include(x => x.Company)
                                    .Where(x => x.Id == invite.Id)
                                    .FirstAsync();
                                //create user and disable token
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
            throw new ArgumentNullException();
        }

        // PUT: api/Invites/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        [Route(Routes.InviteUpdate)]
        public async Task<ActionResult<Invite>> PutInvite(
            [FromBody]Invite inputInvite,
            [FromHeader(Name = "Authorization")] string inputToken,
            [FromRoute] int id)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id == inputInvite.Id)
                    if (permissionCheck.IsAllowed(inputToken, "root"))
                    {
                        var oldInvite = await _context.Invites.FindAsync(id);
                        if (oldInvite == null)
                            throw new ArgumentNullException();

                        inputInvite.CreatedAt = oldInvite.CreatedAt;
                        _context.Entry(oldInvite).State = EntityState.Detached;

                        _context.Invites.Update(inputInvite);
                        await _context.SaveChangesAsync();

                        _context.Entry(inputInvite).State = EntityState.Detached;
                        return await _context.Invites
                            .Include(x => x.Company)
                            .Where(x => x.Id == id)
                            .FirstAsync();
                    }
            throw new ArgumentNullException();
        }


        // DELETE: api/Invites/5
        [HttpDelete]
        [Route(Routes.InviteDelete)]
        public async Task<ActionResult<Invite>> DeleteInvite(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                    if (permissionCheck.IsAllowed(inputToken, "root"))
                    {
                        var invite = await _context.Invites.FindAsync(id);

                        if (invite == null)
                            throw new ArgumentNullException();

                        invite.DeletedAt = DateTimeOffset.UtcNow;
                        _context.Invites.Update(invite);
                        await _context.SaveChangesAsync();

                        _context.Entry(invite).State = EntityState.Detached;

                        return await _context.Invites
                            .IgnoreQueryFilters()
                            .Include(x => x.Company)
                            .Where(x => x.Id == id)
                            .FirstAsync();
                    }
            throw new ArgumentNullException();
        }
    }
}
