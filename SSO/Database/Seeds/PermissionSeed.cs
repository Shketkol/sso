using SSO.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Database.Seeds
{
    /// <summary>
    /// Class to set Seeds into DataBase Table.
    /// </summary>
    public class PermissionSeed
    {
        /// <summary>
        /// Method to set Seeds into Permissions-Table.
        /// </summary>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        public static void SetSeeds(SSOContext db)
        {
            List<Permission> seedList = new List<Permission>
                    {
                        new Permission {Slug =  "set_permissions_to_role",       Table = "common"},
                        new Permission {Slug =  "set_roles_to_user",             Table = "common"},
                        new Permission {Slug =  "edit_roles",                    Table = "common"},
                        new Permission {Slug =  "edit_permissions",              Table = "common"},
                        new Permission {Slug =  "edit_companies",                Table = "common"},
                        new Permission {Slug =  "edit_user_params",              Table = "common"},
                        new Permission {Slug =  "edit_user",                     Table = "common"},

                        new Permission {Slug =  "Root",                          Table = "common"},
                        new Permission {Slug =  "View logs",                     Table = "common"},
                        new Permission {Slug =  "Edit groups",                   Table = "common"},
                        new Permission {Slug =  "Edit users",                    Table = "common"},
                        new Permission {Slug =  "Edit services",                 Table = "common"},
                        new Permission {Slug =  "Edit airline",                  Table = "common"},
                        new Permission {Slug =  "Edit airport",                  Table = "common"},
                        new Permission {Slug =  "Edit arrival",                  Table = "common"},
                        new Permission {Slug =  "Edit departure",                Table = "common"},
                        new Permission {Slug =  "Edit code of delay",            Table = "common"},
                        new Permission {Slug =  "Edit screenplay",               Table = "common"},
                        new Permission {Slug =  "Edit registration",             Table = "common"},
                        new Permission {Slug =  "Edit slide" ,                   Table = "common"},
                        new Permission {Slug =  "Edit aircraft",                 Table = "common"},
                        new Permission {Slug =  "Edit flight",                   Table = "common"},
                        new Permission {Slug =  "Edit plan",                     Table = "common"},
                        new Permission {Slug =  "Edit cancel",                   Table = "common"},
                        new Permission {Slug =  "Edit gate",                     Table = "common"},
                        new Permission {Slug =  "Edit profile",                  Table = "common"},
                        new Permission {Slug =  "Edit cargo",                    Table = "common"},
                        new Permission {Slug =  "Edit past days flight records", Table = "common"},
                        //
                        new Permission {Slug =  "enabled",                       Table = "arrivals"},
//new Permission {Slug =  "cancelled",Table = "arrivals"},
                        new Permission {Slug =  "Airport (not air hub)",         Table = "arrivals"},
                        new Permission {Slug =  "Airport Terminal",              Table = "arrivals"},
                        new Permission {Slug =  "Air hub (not airport)",         Table = "arrivals"},
                        new Permission {Slug =  "Flight status",                 Table = "arrivals"},
                        new Permission {Slug =  "Flight state",                  Table = "arrivals"},
                        new Permission {Slug =  "Flight plan",                   Table = "arrivals"},
                        new Permission {Slug =  "Aircraft",                      Table = "arrivals"},
                        new Permission {Slug =  "Airline",                       Table = "arrivals"},
                        new Permission {Slug =  "Airport stand",                 Table = "arrivals"},
                        new Permission {Slug =  "Registration number",           Table = "arrivals"},
                        new Permission {Slug =  "Aircraft layout",               Table = "arrivals"},
                        new Permission {Slug =  "Schedule Time Departure",       Table = "arrivals"},
                        new Permission {Slug =  "Actual Time Departure",         Table = "arrivals"},
                        new Permission {Slug =  "New Time Departure",            Table = "arrivals"},
                        new Permission {Slug =  "Schedule Time Arrival",         Table = "arrivals"},
                        new Permission {Slug =  "Estimated Time Arrival",        Table = "arrivals"},
                        new Permission {Slug =  "New Estimated Time Arrival",    Table = "arrivals"},
                        new Permission {Slug =  "Actual Time Arrival",           Table = "arrivals"},
                        new Permission {Slug =  "Chocks On",                     Table = "arrivals"},
                        new Permission {Slug =  "Duplicate from cancelled",      Table = "arrivals"},
                        new Permission {Slug =  "Economy Class",                 Table = "arrivals"},
                        new Permission {Slug =  "Business Class",                Table = "arrivals"},
                        new Permission {Slug =  "Special Class",                 Table = "arrivals"},
                        new Permission {Slug =  "Transit Passengers",            Table = "arrivals"},
                        new Permission {Slug =  "Unaccompanied Minors",          Table = "arrivals"},
                        new Permission {Slug =  "Property Irregularity Report",  Table = "arrivals"},
                        new Permission {Slug =  "Baggage",                       Table = "arrivals"},
                        new Permission {Slug =  "Cargo",                         Table = "arrivals"},
                        new Permission {Slug =  "Mail",                          Table = "arrivals"},
                        new Permission {Slug =  "Service Information Message",   Table = "arrivals"},
                        new Permission {Slug =  "Luggage on the tape",           Table = "arrivals"},
                        new Permission {Slug =  "Luggage tapes",                 Table = "arrivals"},
                        //
                        new Permission {Slug =  "enabled",                       Table = "departures"},
//new Permission {Slug =  "cancelled",Table = "departures"},
                        new Permission {Slug =  "Airport (not air hub)",         Table = "departures"},
                        new Permission {Slug =  "Airport Terminal",              Table = "departures"},
                        new Permission {Slug =  "Air hub (not airport)",         Table = "departures"},
                        new Permission {Slug =  "Flight status",                 Table = "departures"},
                        new Permission {Slug =  "Flight state",                  Table = "departures"},
                        new Permission {Slug =  "Flight plan",                   Table = "departures"},
                        new Permission {Slug =  "Aircraft",                      Table = "departures"},
                        new Permission {Slug =  "Airline",                       Table = "departures"},
                        new Permission {Slug =  "Airport stand",                 Table = "departures"},
                        new Permission {Slug =  "Registration number",           Table = "departures"},
                        new Permission {Slug =  "Aircraft layout",               Table = "departures"},
                        new Permission {Slug =  "Schedule Time Departure",       Table = "departures"},
                        new Permission {Slug =  "Actual Time Departure",         Table = "departures"},
                        new Permission {Slug =  "New Time Departure",            Table = "departures"},
                        new Permission {Slug =  "Schedule Time Arrival",         Table = "departures"},
                        new Permission {Slug =  "Schedule Start Check In",       Table = "departures"},
                        new Permission {Slug =  "Start Check In",                Table = "departures"},
                        new Permission {Slug =  "End Check In",                  Table = "departures"},
                        new Permission {Slug =  "Aircraft Ready",                Table = "departures"},
                        new Permission {Slug =  "Boarding start",                Table = "departures"},
                        new Permission {Slug =  "Boarding closed",               Table = "departures"},
                        new Permission {Slug =  "Loading Baggage Closed",        Table = "departures"},
                        new Permission {Slug =  "Trap Cleaning Time",            Table = "departures"},
                        new Permission {Slug =  "Last Passenger Time",           Table = "departures"},
                        new Permission {Slug =  "Chocks Off",                    Table = "departures"},
                        new Permission {Slug =  "Take Off Time",                 Table = "departures"},
                        new Permission {Slug =  "Time Delay",                    Table = "departures"},
                        new Permission {Slug =  "Duplicate from cancelled",      Table = "departures"},
                        new Permission {Slug =  "Passenger Preloading",          Table = "departures"},
                        new Permission {Slug =  "Economy Class",                 Table = "departures"},
                        new Permission {Slug =  "Business Class",                Table = "departures"},
                        new Permission {Slug =  "Special Class",                 Table = "departures"},
                        new Permission {Slug =  "Transit Passengers",            Table = "departures"},
                        new Permission {Slug =  "Unaccompanied Minors",          Table = "departures"},
                        new Permission {Slug =  "Passengers To Pay",             Table = "departures"},
                        new Permission {Slug =  "Infant Passengers",             Table = "departures"},
                        new Permission {Slug =  "Staff Passengers",              Table = "departures"},
                        new Permission {Slug =  "Depot Passengers",              Table = "departures"},
                        new Permission {Slug =  "Other Passengers",              Table = "departures"},
                        new Permission {Slug =  "Baggage",                       Table = "departures"},
                        new Permission {Slug =  "Cargo",                         Table = "departures"},
                        new Permission {Slug =  "Mail",                          Table = "departures"},
                        new Permission {Slug =  "Service Information Message",   Table = "departures"},
                        new Permission {Slug =  "If code delay issue is solved", Table = "departures"}
                    };

            foreach (var item in seedList)
            {
                item.Slug = item.Slug.Replace(' ', '-').ToLower();
                db.Permissions.Add(item);
            }
            db.SaveChanges();
        }

    }
}
