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
        /// <summary>  Command to update a save game.</summary>
        /// <value>The update save game.</value>
        public ICommand UpdateSaveGame { get; set; }

        /// <summary>The media objects data access</summary>
        public MediaObjects MediaObjectsDataAccess = new MediaObjects();
        /// <summary>The crew member data access</summary>
        public CrewMembers CrewMemberDataAccess = new CrewMembers();
        /// <summary>The answers data access</summary>
        public DataAccess.Answers AnswersDataAccess = new DataAccess.Answers();
        /// <summary>The game profiles data access</summary>
        public GameProfiles GameProfilesDataAccess = new GameProfiles();

        /// <summary>Initializes a new instance of the <see cref="AdminViewModel" /> class.</summary>
        public AdminViewModel()
        {
            UpdateSaveGame = new RelayCommand<UsingChallenge>(async param =>
                                                    {
                                                        await GameProfilesDataAccess.AddNewEntryToSaveGameAsync(param);
                                                    }, param => param != null);
        }

        /// <summary>Loads the object from database asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal async Task<MediaObject> LoadObjectFromDBAsync(int? id, string type)
        {
            return await MediaObjectsDataAccess.GetMediaObjectAsync(id, type);
        }

        /// <summary>Loads the crew member from database asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        internal async Task<CrewMember> LoadCrewMemberFromDBAsync(int? id)
        {
            return await CrewMemberDataAccess.GetCrewMemberAsync(id);
        }

        /// <summary>Loads the answers from database asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        internal async Task<Modell.ChallengeObjects.Answers> LoadAnswersFromDBAsync(int? id)
        {
            return await AnswersDataAccess.GetAnswersAsync(id);
        }
    }
}
