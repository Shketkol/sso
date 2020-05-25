using System;
using System.Collections.Generic;
using System.Linq;
using SSO.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using SSO.Services.Abstract;
using SSO.Models.Errors;
using System.Threading.Tasks;
using SSO.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using SSO.Database.Functional;

namespace SSO.DatabaseControllers
{
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ITokenCheck tokenCheck;
        private readonly IPermissionCheck permissionCheck;

        public CompaniesController(ITokenCheck tokenCheck, IPermissionCheck permissionCheck)
        {
            this.tokenCheck = tokenCheck;
            this.permissionCheck = permissionCheck;
        }

        /// <summary>
        /// Method to get all records from Companies-table.
        /// Calls method <see cref="GetAllCompanies()"/>.
        /// </summary>
        /// <returns>
        /// <see cref="List{Company}"/>.
        /// </returns>
        [HttpGet]
        [Route(Routes.CompanyList)]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                using var db = new SSOContext();
                var companyList = await (from companies in db.Companies
                                         select companies).ToListAsync();

                if (companyList.Any())
                    return companyList;
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get a specific record from Companies-table.
        /// </summary>
        /// <returns>
        /// <see cref="Company"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">Company record identificator. Integer.</param>
        [HttpGet]
        [Route(Routes.CompanyItem)]
        public async Task<ActionResult<Company>> GetCompany(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var company = await (from companies in db.Companies
                                         where companies.Id == id
                                         select companies).FirstAsync();

                    if (company != null)
                        return company;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to create a specific record for Companies-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Company"/> object is null.</exception>
        /// <param name="company"><see cref="Company"/> object.</param>
        [HttpPost]
        [Route(Routes.CompanyCreate)]
        public async Task<ActionResult<Company>> InsertCompany([FromBody]Company company, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (company != null)
                    if (permissionCheck.IsAllowed(inputToken, "edit_companies"))
                    {
                        using var db = new SSOContext();
                        db.Companies.Add(company);
                        await db.SaveChangesAsync();
                        db.Entry(company).State = EntityState.Detached;
                        return await db.Companies.FindAsync(company.Id);
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to update a specific record from Companies-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Company"/> object is null.</exception>
        /// <param name="company"><see cref="Company"/> object.</param>
        [HttpPut]
        [Route(Routes.CompanyUpdate)]
        public async Task<ActionResult<Company>> UpdateCompany([FromBody]Company company,
            [FromHeader(Name = "Authorization")] string inputToken,
            [FromRoute] int id)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id == company.Id)
                    if (permissionCheck.IsAllowed(inputToken, "edit_companies"))
                    {
                        using var db = new SSOContext();
                        var oldCompany = await db.Companies.FindAsync(id);
                        if (oldCompany == null)
                            throw new ArgumentNullException();

                        company.CreatedAt = oldCompany.CreatedAt;
                        db.Entry(oldCompany).State = EntityState.Detached;

                        db.Companies.Update(company);
                        await db.SaveChangesAsync();

                        db.Entry(company).State = EntityState.Detached;
                        return await db.Companies.FindAsync(id);
                    }

            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to delete a specific record from Companies-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Company"/> object is null.</exception>
        /// <param name="id">Company record identificator. Integer.</param>
        [HttpDelete]
        [Route(Routes.CompanyDelete)]
        public async Task<ActionResult<Company>> DeleteCompany(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                    if (permissionCheck.IsAllowed(inputToken, "edit_companies"))
                    {
                        using var db = new SSOContext();
                        var getCompany = await (from companies in db.Companies
                                                where companies.Id == id
                                                select companies).FirstAsync();

                        if (getCompany == null)
                            throw new ArgumentNullException();

                        getCompany.DeletedAt = DateTimeOffset.UtcNow;
                        db.Companies.Update(getCompany);
                        await db.SaveChangesAsync();

                        db.Entry(getCompany).State = EntityState.Detached;
                        return await db.Companies
                            .IgnoreQueryFilters()
                            .Where(x => x.Id == id).FirstAsync();
                    }
            throw new ArgumentNullException();
        }
    }
}
