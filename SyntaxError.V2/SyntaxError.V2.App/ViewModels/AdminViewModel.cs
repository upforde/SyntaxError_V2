using System;
using System.Threading.Tasks;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.App.ViewModels
{
    public class AdminViewModel : Observable
    {
        public MediaObjects MediaObjectsDataAccess = new MediaObjects();
        public CrewMembers CrewMemberDataAccess = new CrewMembers();
        public DataAccess.Answers AnswersDataAccess = new DataAccess.Answers();

        public AdminViewModel()
        {
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
