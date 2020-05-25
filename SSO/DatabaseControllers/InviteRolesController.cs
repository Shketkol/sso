using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Database;
using SSO.Models.DatabaseModels;
using SSO.Services.Abstract;

namespace SSO.DatabaseControllers
{
    [ApiController]
    public class InviteRolesController : ControllerBase
    {
        private readonly SSOContext _context;
        private readonly ITokenCheck tokenCheck;
        private readonly IPermissionCheck permissionCheck;

        public InviteRolesController(SSOContext context, ITokenCheck tokenCheck, IPermissionCheck permissionCheck)
        {
            _context = context;
            this.tokenCheck = tokenCheck;
            this.permissionCheck = permissionCheck;
        }

        [HttpGet]
        [Route(Routes.InviteRolesByRole)]
        public async Task<ActionResult<IEnumerable<InviteRole>>> GetInviteRoleByRole(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    var output = await _context.InviteRoles
                        .Include(x => x.Role)
                        .Include(x => x.Invite)
                        .Where(x => x.RoleId == id)
                        .ToListAsync();

                    if (output.Any())
                        return output;
                }
            throw new ArgumentNullException();
        }

        [HttpGet]
        [Route(Routes.InviteRolesByInvite)]
        public async Task<ActionResult<IEnumerable<InviteRole>>> GetInviteRoleByInvite(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    var output = await _context.InviteRoles
                        .Include(x => x.Role)
                        .Include(x => x.Invite)
                        .Where(x => x.InviteId == id)
                        .ToListAsync();

                    if (output.Any())
                        return output;
                }
            throw new ArgumentNullException();
        }

        // GET: api/InviteRoles
        [HttpGet]
        [Route(Routes.InviteRolesList)]
        public async Task<ActionResult<IEnumerable<InviteRole>>> GetInviteRoles([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                var output = await _context.InviteRoles
                    .Include(x => x.Role)
                    .Include(x => x.Invite)
                    .ToListAsync();

                if (output.Any())
                    return output;
            }
            throw new ArgumentNullException();
        }

        // GET: api/InviteRoles/5
        [HttpGet]
        [Route(Routes.InviteRolesItem)]
        public async Task<ActionResult<InviteRole>> GetInviteRole(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    var inviteRole = await _context.InviteRoles
                        .Include(x => x.Role)
                        .Include(x => x.Invite)
                        .FirstAsync();

                    if (inviteRole != null)
                        return inviteRole;
                }
            throw new ArgumentNullException();
        }

        // PUT: api/InviteRoles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        [Route(Routes.InviteRolesUpdate)]
        public async Task<ActionResult<InviteRole>> PutInviteRole([FromBody]InviteRole inviteRole,
            [FromHeader(Name = "Authorization")] string inputToken,
            [FromRoute] int id)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id == inviteRole.Id)
                    if (permissionCheck.IsAllowed(inputToken, "root"))
                    {
                        var oldInviteRole = await _context.InviteRoles.FindAsync(id);
                        if (oldInviteRole == null)
                            throw new ArgumentNullException();

                        inviteRole.CreatedAt = oldInviteRole.CreatedAt;
                        _context.Entry(oldInviteRole).State = EntityState.Detached;

                        _context.InviteRoles.Update(inviteRole);
                        await _context.SaveChangesAsync();

                        _context.Entry(inviteRole).State = EntityState.Detached;
                        return await _context.InviteRoles
                            .Include(x => x.Role)
                            .Include(x => x.Invite)
                            .Where(x => x.Id == id)
                            .FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        // POST: api/InviteRoles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route(Routes.InviteRolesCreate)]
        public async Task<ActionResult<InviteRole>> PostInviteRole([FromBody] InviteRole inviteRole, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (inviteRole != null)
                    if (permissionCheck.IsAllowed(inputToken, "root"))
                    {
                        _context.InviteRoles.Add(inviteRole);
                        await _context.SaveChangesAsync();

                        _context.Entry(inviteRole).State = EntityState.Detached;

                        return await _context.InviteRoles
                            .Include(x => x.Role)
                            .Include(x => x.Invite)
                            .Where(x => x.Id == inviteRole.Id)
                            .FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        // DELETE: api/InviteRoles/5
        [HttpDelete]
        [Route(Routes.InviteRolesDelete)]
        public async Task<ActionResult<InviteRole>> DeleteInviteRole(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                    if (permissionCheck.IsAllowed(inputToken, "root"))
                    {
                        var inviteRole = await _context.InviteRoles.FindAsync(id);

                        if (inviteRole == null)
                            throw new ArgumentNullException();

                        inviteRole.DeletedAt = DateTimeOffset.UtcNow;
                        _context.InviteRoles.Update(inviteRole);
                        await _context.SaveChangesAsync();

                        _context.Entry(inviteRole).State = EntityState.Detached;

                        return await _context.InviteRoles
                            .IgnoreQueryFilters()
                            .Include(x => x.Role)
                            .Include(x => x.Invite)
                            .Where(x => x.Id == inviteRole.Id)
                            .FirstAsync();
                    }
            throw new ArgumentNullException();
        }
    }
}
