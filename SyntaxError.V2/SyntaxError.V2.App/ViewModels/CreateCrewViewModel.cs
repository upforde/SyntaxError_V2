using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.App.ViewModels
{
    public class CreateCrewViewModel : Observable
    {
        /// <summary>A command to edit a CrewMember in the database and list.</summary>
        /// <value>The edit command.</value>
        public ICommand EditCommand { get; set; }
        /// <summary>A command to delete a new CrewMember from the database and list.</summary>
        /// <value>The delete command.</value>
        public ICommand DeleteCommand { get; set; }

        /// <summary>The crew members</summary>
        public List<CrewMember> CrewMembers = new List<CrewMember>();

        /// <summary>The crew members data access</summary>
        public CrewMembers CrewMembersDataAccess = new DataAccess.CrewMembers();

        /// <summary>Initializes a new instance of the <see cref="CreateCrewViewModel" /> class.</summary>
        public CreateCrewViewModel()
        {
            EditCommand = new RelayCommand<CrewMember>(async param =>
                                                    {
                                                        await CrewMembersDataAccess.EditCrewMemberAsync(param);
                                                    }, param => param != null);

            DeleteCommand = new RelayCommand<CrewMember>(async param =>
                                                    {
                                                        if (await CrewMembersDataAccess.DeleteCrewMemberAsync(param))
                                                        {
                                                            CrewMembers.Remove(param);
                                                        }
                                                    }, param => param != null);
        }

        /// <summary>Loads the crew members from database asynchronous.</summary>
        /// <returns></returns>
        internal async Task<bool> LoadCrewMembersFromDBAsync()
        {
                if (CrewMembers.Count == 0)
                {
                    var crewMembers = await CrewMembersDataAccess.GetCrewMembersAsync();
                    foreach (CrewMember crewMember in crewMembers)
                        CrewMembers.Add(crewMember);
                }
                return true;
        
        }
    }
}
