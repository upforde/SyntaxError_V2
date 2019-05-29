using System;
using System.Collections.ObjectModel;
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
        
        public ObservableCollection<CrewMember> CrewMembers = new ObservableCollection<CrewMember>();
        
        public CrewMembers CrewMembersDataAccess = new DataAccess.CrewMembers();
        public Images ImagesDataAccess = new DataAccess.Images();

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

        internal async Task LoadCrewMembersFromDBAsync()
        {
            if (CrewMembers.Count == 0)
            {
                var crewMembers = await CrewMembersDataAccess.GetCrewMembersAsync();
                foreach (CrewMember crewMember in crewMembers)
                    CrewMembers.Add(crewMember);
            }
        }
    }
}
