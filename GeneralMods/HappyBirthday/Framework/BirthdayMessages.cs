using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Omegasis.HappyBirthday.Framework.ContentPack;
using Omegasis.HappyBirthday.Framework.Utilities;
using StardewModdingAPI;
using StardewValley;
using static StardewValley.LocalizedContentManager;

namespace Omegasis.HappyBirthday
{
    /// <summary>
    /// A class that deals with handling getting birthday wishes for the mod.
    /// </summary>
    public class BirthdayMessages
    {

        public BirthdayMessages()
        {
        }

        /// <summary>
        /// Loads some strings from the StringsFromCS file for affectionate spouse words.
        /// </summary>
        /// <returns></returns>
        protected virtual string getAffectionateSpouseWord()
        {

            List<string> words = new List<string>();
            string dict = Path.Combine("Strings", "StringsFromCSFiles");
            if (Game1.player.IsMale)
            {

                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4507"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4509"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4511"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4514"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4515"));


            }
            else
            {
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4512"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4513"));

            }
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4508"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4510"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4516"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4517"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4518"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4519"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4522"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4523"));

            if (LocalizedContentManager.CurrentLanguageCode== LanguageCode.en)
            {
                words.Add("Pumpkin"); //Because this is cute.
            }

            string s = words[Game1.random.Next(0, words.Count - 1)];

            return s.ToLowerInvariant();
        }

        /// <summary>
        /// Gets an appropriate time of day string depending on the time of day.
        /// </summary>
        /// <returns></returns>
        protected virtual string getTimeOfDayString()
        { 
            if (Game1.timeOfDay >= 600 && Game1.timeOfDay < 1200)
            {
                return "morning";
            }
            else if (Game1.timeOfDay >= 1200 && Game1.timeOfDay < 600)
            {
                return "afternoon";
            }
            else return "evening";
        }

        /// <summary>
        /// Gets the message from a given npc/spouse for the player.
        /// </summary>
        /// <param name="NPC"></param>
        /// <returns></returns>
        public virtual string getBirthdayMessage(string NPC)
        {
            if (Game1.player.friendshipData.ContainsKey(NPC))
            {
                if (Game1.player.getSpouse() != null) {
                    if (Game1.player.getSpouse().Name.Equals(NPC))
                    {
                        return this.getSpouseBirthdayWish(NPC);
                    }
                    else
                    {
                        return this.getNonSpouseBirthdayWish(NPC);
                    }
                }
                else
                {
                    return this.getNonSpouseBirthdayWish(NPC);
                }

            }
            else
            {
                //Potentially don't grab birthday wishes here if the npc is not know by the player?
                return this.getNonSpouseBirthdayWish(NPC);
            }
        }

        /// <summary>
        /// Gets a non-spouse birthday wish.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public virtual string getNonSpouseBirthdayWish(string Key)
        {
            return this.getNonSpouseBirthdayWish(Key, LocalizationUtilities.GetCurrentLanguageCodeString(), true);
        }

        /// <summary>
        /// Gets a non spouse birthday wish.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="LanguageCode"></param>
        /// <param name="DefaultToEnglish"></param>
        /// <returns></returns>
        public virtual string getNonSpouseBirthdayWish(string Key, string LanguageCode, bool DefaultToEnglish)
        {
            if (LanguageCode == LocalizationUtilities.GetEnglishLanguageCode() && DefaultToEnglish == true)
            {
                //Prevent infinite recursion.
                DefaultToEnglish = false;
            }
            List<HappyBirthdayContentPack> affectedContentPacks = HappyBirthday.Instance.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode();
            List<string> potentialBirthdayWishes = new List<string>();
            foreach (HappyBirthdayContentPack contentPack in affectedContentPacks)
            {
                string birthdayWish = contentPack.getBirthdayWish(Key, false);
                if (string.IsNullOrEmpty(birthdayWish)) continue;
                potentialBirthdayWishes.Add(birthdayWish);
            }

            if (potentialBirthdayWishes.Count == 0)
            {
                if (DefaultToEnglish)
                {
                    return this.getNonSpouseBirthdayWish(Key, LocalizationUtilities.GetEnglishLanguageCode(), false);
                }

                return this.getDefaultBirthdayWish();
            }
            else
            {
                int randomWishIndex = Game1.random.Next(0, potentialBirthdayWishes.Count);
                return potentialBirthdayWishes[randomWishIndex];
            }
        }


        /// <summary>
        /// Gets a spouse birthday wish.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public virtual string getSpouseBirthdayWish(string Key)
        {
            return this.getSpouseBirthdayWish(Key, LocalizationUtilities.GetCurrentLanguageCodeString(), true);
        }

        /// <summary>
        /// Gets a spouse birthday wish.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="LanguageCode"></param>
        /// <param name="DefaultToEnglish"></param>
        /// <returns></returns>
        public virtual string getSpouseBirthdayWish(string Key, string LanguageCode, bool DefaultToEnglish)
        {
            if (LanguageCode == LocalizationUtilities.GetEnglishLanguageCode() && DefaultToEnglish == true)
            {
                //Prevent infinite recursion.
                DefaultToEnglish = false;
            }
            List<HappyBirthdayContentPack> affectedContentPacks = HappyBirthday.Instance.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode();
            List<string> potentialBirthdayWishes = new List<string>();
            foreach (HappyBirthdayContentPack contentPack in affectedContentPacks)
            {
                string birthdayWish = contentPack.getSpouseBirthdayWish(Key, false);
                if (string.IsNullOrEmpty(birthdayWish)) continue;
                potentialBirthdayWishes.Add(birthdayWish);
            }
            if (potentialBirthdayWishes.Count == 0)
            {
                if (DefaultToEnglish)
                {
                   string spouseBirthdayWish= this.getSpouseBirthdayWish(Key, LocalizationUtilities.GetEnglishLanguageCode(), false);

                    spouseBirthdayWish = spouseBirthdayWish.Replace("{AffectionateSpouseWord}", this.getAffectionateSpouseWord());
                    spouseBirthdayWish = spouseBirthdayWish.Replace("{TimeOfDay}", this.getTimeOfDayString());
                    spouseBirthdayWish = spouseBirthdayWish.Replace("@", Game1.player.Name);

                    return spouseBirthdayWish;


                }
                return this.getNonSpouseBirthdayWish(Key);
            }
            else
            {
                int randomWishIndex = Game1.random.Next(0, potentialBirthdayWishes.Count);
                return potentialBirthdayWishes[randomWishIndex];
            }
        }

        /// <summary>
        /// Gets the default birthday wish message.
        /// </summary>
        /// <returns></returns>
        public virtual string getDefaultBirthdayWish()
        {
            return this.getDefaultBirthdayWish(LocalizationUtilities.GetCurrentLanguageCodeString(), true);

        }

        /// <summary>
        /// Gets the default birthday wish message.
        /// </summary>
        /// <returns></returns>
        public virtual string getDefaultBirthdayWish(string LanguageCode, bool DefaultToEnglish)
        {
            string Key = "Default Birthday Wish";
            if (LanguageCode == LocalizationUtilities.GetEnglishLanguageCode() && DefaultToEnglish == true)
            {
                //Prevent infinite recursion.
                DefaultToEnglish = false;
            }
            List<HappyBirthdayContentPack> affectedContentPacks = HappyBirthday.Instance.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode();
            List<string> potentialBirthdayWishes = new List<string>();
            foreach (HappyBirthdayContentPack contentPack in affectedContentPacks)
            {
                string birthdayWish = contentPack.getTranslationString(Key, false);
                if (string.IsNullOrEmpty(birthdayWish)) continue;
                potentialBirthdayWishes.Add(birthdayWish);
            }

            if (potentialBirthdayWishes.Count == 0)
            {
                if (DefaultToEnglish)
                {
                    return this.getDefaultBirthdayWish(LocalizationUtilities.GetEnglishLanguageCode(), false);
                }

                return "Happy Birthday @!";
            }
            else
            {
                int randomWishIndex = Game1.random.Next(0, potentialBirthdayWishes.Count);
                return potentialBirthdayWishes[randomWishIndex];
            }

        }
    }
}


