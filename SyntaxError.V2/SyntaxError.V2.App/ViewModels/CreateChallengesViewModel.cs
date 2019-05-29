﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.Modell.Challenges;

namespace SyntaxError.V2.App.ViewModels
{
    public class CreateChallengesViewModel : Observable
    {
        public ObservableCollection<AudienceChallenge> AudienceChallenges { get; set; } = new ObservableCollection<AudienceChallenge>();
        public ObservableCollection<CrewChallenge> CrewChallenges { get; set; } = new ObservableCollection<CrewChallenge>();
        public ObservableCollection<MultipleChoiceChallenge> MultipleChoiceChallenges { get; set; } = new ObservableCollection<MultipleChoiceChallenge>();
        public ObservableCollection<MusicChallenge> MusicChallenges { get; set; } = new ObservableCollection<MusicChallenge>();
        public ObservableCollection<QuizChallenge> QuizChallenges { get; set; } = new ObservableCollection<QuizChallenge>();
        public ObservableCollection<ScreenshotChallenge> ScreenshotChallenges { get; set; } = new ObservableCollection<ScreenshotChallenge>();
        public ObservableCollection<SilhouetteChallenge> SilhouetteChallenges { get; set; } = new ObservableCollection<SilhouetteChallenge>();
        public ObservableCollection<SologameChallenge> SologameChallenges { get; set; } = new ObservableCollection<SologameChallenge>();

        public Challenges challengesDataAccess = new Challenges();
        
        public List<ChallengeBase> ChallengesFromDB = new List<ChallengeBase>();

        public CreateChallengesViewModel()
        {
        }

        /// <summary>Loads the challenges from database asynchronous.</summary>
        /// <returns></returns>
        internal async Task LoadChallengesFromDBAsync()
        {
            var challenges = await challengesDataAccess.GetChallengesAsync();
            foreach (ChallengeBase challenge in challenges)
            {
                switch (challenge.GetDiscriminator())
                {
                    case "AudienceChallenge":
                        AudienceChallenges.Add(challenge as AudienceChallenge);
                        break;
                    case "CrewChallenge":
                        CrewChallenges.Add(challenge as CrewChallenge);
                        break;
                    case "MultipleChoiceChallenge":
                        MultipleChoiceChallenges.Add(challenge as MultipleChoiceChallenge);
                        break;
                    case "MusicChallenge":
                        MusicChallenges.Add(challenge as MusicChallenge);
                        break;
                    case "QuizChallenge":
                        QuizChallenges.Add(challenge as QuizChallenge);
                        break;
                    case "ScreenshotChallenge":
                        ScreenshotChallenges.Add(challenge as ScreenshotChallenge);
                        break;
                    case "SilhouetteChallenge":
                        SilhouetteChallenges.Add(challenge as SilhouetteChallenge);
                        break;
                    case "SologameChallenge":
                        SologameChallenges.Add(challenge as SologameChallenge);
                        break;
                }
            }
        }
    }
}
