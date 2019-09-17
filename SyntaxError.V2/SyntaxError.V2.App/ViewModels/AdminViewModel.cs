using System;
using System.Threading.Tasks;
using System.Windows.Input;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Utility;

namespace SyntaxError.V2.App.ViewModels
{
    public class AdminViewModel : Observable
    {
        public ICommand EditCommand { get; set; }

        public MediaObjects MediaObjectsDataAccess = new MediaObjects();
        public CrewMembers CrewMemberDataAccess = new CrewMembers();
        public DataAccess.Answers AnswersDataAccess = new DataAccess.Answers();
        public GameProfiles GameProfilesDataAccess = new GameProfiles();

        public AdminViewModel()
        {
            EditCommand = new RelayCommand<GameProfile>(async param =>
                                                    {
                                                        await GameProfilesDataAccess.EditGameProfileAsync(param);
                                                    }, param => param != null);
        }

        internal async Task<MediaObject> LoadObjectFromDBAsync(int? id, string type)
        {
            return await MediaObjectsDataAccess.GetMediaObjectAsync(id, type);
        }

        internal async Task<CrewMember> LoadCrewMemberFromDBAsync(int? id)
        {
            return await CrewMemberDataAccess.GetCrewMemberAsync(id);
        }

        internal async Task<Modell.ChallengeObjects.Answers> LoadAnswersFromDBAsync(int? id)
        {
            return await AnswersDataAccess.GetAnswersAsync(id);
        }
    }
}
