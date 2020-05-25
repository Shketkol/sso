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
    public class CompanyAirportsController : ControllerBase
    {
        private readonly SSOContext _context;
        private readonly ITokenCheck tokenCheck;
        private readonly IPermissionCheck permissionCheck;

        public CompanyAirportsController(SSOContext context, ITokenCheck tokenCheck, IPermissionCheck permissionCheck)
        {
            _context = context;
            this.tokenCheck = tokenCheck;
            this.permissionCheck = permissionCheck;
        }

        /// <summary>
        /// GET: /company-airports/company/{id}
        /// <para>Method to get all CompanyAirportsSet-table records by company id.</para>
        /// </summary>
        /// <returns>
        /// <see cref="List{CompanyAirports}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0 or token is invalid.</exception>
        /// <param name="id">Company id.</param>
        /// <param name="inputToken">JWT token.</param>
        [HttpGet]
        [Route(Routes.CompanyAirportsByCompany)]
        public async Task<ActionResult<IEnumerable<CompanyAirports>>> GetCompanyAirportsByCompany(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    var output = await _context.CompanyAirportsSet
                        .Include(x => x.Company)
                        .Where(x => x.CompanyId == id)
                        .ToListAsync();

                    if (output.Any())
                        return output;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// GET: /company-airports
        /// <para>Method to get all CompanyAirportsSet-table records.</para>
        /// </summary>
        /// <returns>
        /// <see cref="List{CompanyAirports}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when token is invalid.</exception>
        /// <param name="inputToken">JWT token.</param>
        [HttpGet]
        [Route(Routes.CompanyAirportsList)]
        public async Task<ActionResult<IEnumerable<CompanyAirports>>> GetAllCompanyAirports([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                var output = await _context.CompanyAirportsSet
                    .Include(x => x.Company)
                    .ToListAsync();

                if (output.Any())
                    return output;
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// GET: /company-airports/{id}
        /// <para>Method to get CompanyAirportsSet-table record by its id.</para>
        /// </summary>
        /// <returns>
        /// <see cref="{CompanyAirports}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0 or token is invalid.</exception>
        /// <param name="id">CompanyAirports id.</param>
        /// <param name="inputToken">JWT token.</param>
        [HttpGet]
        [Route(Routes.CompanyAirportsItem)]
        public async Task<ActionResult<CompanyAirports>> GetCompanyAirports(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    var companyAirports = await _context.CompanyAirportsSet
                        .Include(x => x.Company)
                        .Where(x => x.Id == id)
                        .FirstAsync();

                    if (companyAirports != null)
                        return companyAirports;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// POST: /company-airports
        /// <para>Method to set CompanyAirportsSet-table record by its input object.</para>
        /// </summary>
        /// <returns>
        /// <see cref="{CompanyAirports}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when token is invalid.</exception>
        /// <param name="companyAirports">CompanyAirports object.</param>
        /// <param name="inputToken">JWT token.</param>
        [HttpPost]
        [Route(Routes.CompanyAirportsCreate)]
        public async Task<ActionResult<CompanyAirports>> PostCompanyAirports([FromBody]CompanyAirports companyAirports, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (companyAirports != null)
                    if (permissionCheck.IsAllowed(inputToken, "root"))
                    {
                        _context.CompanyAirportsSet.Add(companyAirports);
                        await _context.SaveChangesAsync();

                        _context.Entry(companyAirports).State = EntityState.Detached;

                        return await _context.CompanyAirportsSet
                        .Include(x => x.Company)
                        .Where(x => x.Id == companyAirports.Id)
                        .FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// PUT: /company-airports/{id}
        /// <para>Method to update CompanyAirportsSet-table record by its id and input object.</para>
        /// </summary>
        /// <returns>
        /// <see cref="{CompanyAirports}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0 or token is invalid.</exception>
        /// <param name="id">CompanyAirports id.</param>
        /// <param name="companyAirports">CompanyAirports object.</param>
        /// <param name="inputToken">JWT token.</param>
        [HttpPut]
        [Route(Routes.CompanyAirportsUpdate)]
        public async Task<ActionResult<CompanyAirports>> PutCompanyAirports([FromBody]CompanyAirports companyAirports,
            [FromHeader(Name = "Authorization")] string inputToken,
            [FromRoute] int id)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id == companyAirports.Id)
                    if (permissionCheck.IsAllowed(inputToken, "root"))
                    {
                        var oldCompanyAirports = await _context.CompanyAirportsSet.FindAsync(id);
                        if (oldCompanyAirports == null)
                            throw new ArgumentNullException();

                        companyAirports.CreatedAt = oldCompanyAirports.CreatedAt;
                        _context.Entry(oldCompanyAirports).State = EntityState.Detached;

                        _context.CompanyAirportsSet.Update(companyAirports);
                        await _context.SaveChangesAsync();

                        _context.Entry(companyAirports).State = EntityState.Detached;
                        return await _context.CompanyAirportsSet
                            .Include(x => x.Company)
                            .Where(x => x.Id == id)
                            .FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// DELETE: /company-airports/{id}
        /// <para>Method to delete CompanyAirportsSet-table record by its id.</para>
        /// </summary>
        /// <returns>
        /// <see cref="{CompanyAirports}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0 or token is invalid.</exception>
        /// <param name="id">CompanyAirports id.</param>
        /// <param name="inputToken">JWT token.</param>
        [HttpDelete]
        [Route(Routes.CompanyAirportsDelete)]
        public async Task<ActionResult<CompanyAirports>> DeleteCompanyAirports(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                    if (permissionCheck.IsAllowed(inputToken, "root"))
                    {
                        var companyAirports = await _context.CompanyAirportsSet.FindAsync(id);

                        if (companyAirports == null)
                            throw new ArgumentNullException();

                        companyAirports.DeletedAt = DateTimeOffset.UtcNow;
                        _context.CompanyAirportsSet.Update(companyAirports);
                        await _context.SaveChangesAsync();

                        _context.Entry(companyAirports).State = EntityState.Detached;

                        return await _context.CompanyAirportsSet
                            .IgnoreQueryFilters()
                            .Include(x => x.Company)
                            .Where(x => x.Id == id)
                            .FirstAsync();
                    }
            throw new ArgumentNullException();
        }
    }
}
