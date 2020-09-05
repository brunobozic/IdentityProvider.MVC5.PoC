using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IdentityProvider.Infrastructure.LatestAdditions
{

    public class IbanHelper
    {
        private string CountryCode { get; set; }
        public int Length { get; private set; }
        public string Regex { get; private set; }
        private bool IsEu924 { get; set; }
        private string Sample { get; set; }

        public IbanHelper(
            string countryCode
            , int length
            , string regexStructure
            , bool isEu924 = false
            , string sample = ""
        )
        {
            CountryCode = countryCode;
            Length = length;
            Regex = regexStructure;
            IsEu924 = isEu924;
            Sample = sample;
        }
    }

    public class IbanStatus
    {
        public bool IsValid;
        public string Message;

        public IbanStatus(string message, bool isValid = false)
        {
            IsValid = isValid;
            Message = message;
        }
    }

    public static class IbanValidator
    {
        private static class ValidationMessages
        {
            public const string IllegalCharactersFound = "The IBAN contains illegal characters.";
            public const string WrongStructure = "The structure of IBAN is wrong.";
            public const string WrongCheckDigits = "The check digits of IBAN are wrong.";
            public const string CountryIbanNotDefined = "IBAN for country {0} currently is not avaliable.";
            public const string CountryWrongLength = "The IBAN of {0} needs to be {1} characters long.";
            public const string CountryWrongStructure = "The country specific structure of IBAN is wrong.";
            public const string Invalid = "The IBAN is incorrect.";
            public const string Valid = "The IBAN is correct.";
        }

        /// <summary>
        /// http://www.tbg5-finance.org/checkiban.js
        /// </summary>
        public static Dictionary<string, IbanHelper> IbanCountryValidations => new Dictionary<string, IbanHelper>
        {
            { "AD", new IbanHelper("AD", 24, @"\d{8}[a-zA-Z0-9]{12}", false, "AD1200012030200359100100") },
            { "AL", new IbanHelper("AL", 28, @"\d{8}[a-zA-Z0-9]{16}", false, "AL47212110090000000235698741") },
            { "AT", new IbanHelper("AT", 20, @"\d{16}", true, "AT611904300234573201") },
            { "BA", new IbanHelper("BA", 20, @"\d{16}", false, "BA391290079401028494") },
            { "BE", new IbanHelper("BE", 16, @"\d{12}", true, "BE68539007547034") },
            { "BG", new IbanHelper("BG", 22, @"[A-Z]{4}\d{6}[a-zA-Z0-9]{8}", true, "BG80BNBG96611020345678") },
            { "CH", new IbanHelper("CH", 21, @"\d{5}[a-zA-Z0-9]{12}", false, "CH9300762011623852957") },
            { "CY", new IbanHelper("CY", 28, @"\d{8}[a-zA-Z0-9]{16}", true, "CY17002001280000001200527600") },
            { "CZ", new IbanHelper("CZ", 24, @"\d{20}", true, "CZ6508000000192000145399") },
            { "DE", new IbanHelper("DE", 22, @"\d{18}", true, "DE89370400440532013000") },
            { "DK", new IbanHelper("DK", 18, @"\d{14}", true, "DK5000400440116243") },
            { "EE", new IbanHelper("EE", 20, @"\d{16}", true, "EE382200221020145685") },
            { "ES", new IbanHelper("ES", 24, @"\d{20}", true, "ES9121000418450200051332") },
            { "FI", new IbanHelper("FI", 18, @"\d{14}", true, "FI2112345600000785") },
            { "FO", new IbanHelper("FO", 18, @"\d{14}", false, "FO6264600001631634") },
            { "FR", new IbanHelper("FR", 27, @"\d{10}[a-zA-Z0-9]{11}\d\d", true, "FR1420041010050500013M02606") },
            { "GB", new IbanHelper("GB", 22, @"[A-Z]{4}\d{14}", true, "GB29NWBK60161331926819") },
            { "GI", new IbanHelper("GI", 23, @"[A-Z]{4}[a-zA-Z0-9]{15}", true, "GI75NWBK000000007099453") },
            { "GL", new IbanHelper("GL", 18, @"\d{14}", false, "GL8964710001000206") },
            { "GR", new IbanHelper("GR", 27, @"\d{7}[a-zA-Z0-9]{16}", true, "GR1601101250000000012300695") },
            { "HR", new IbanHelper("HR", 21, @"\d{17}", false, "HR1210010051863000160") },
            { "HU", new IbanHelper("HU", 28, @"\d{24}", true, "HU42117730161111101800000000") },
            { "IE", new IbanHelper("IE", 22, @"[A-Z]{4}\d{14}", true, "IE29AIBK93115212345678") },
            { "IL", new IbanHelper("IL", 23, @"\d{19}", false, "IL620108000000099999999") },
            { "IS", new IbanHelper("IS", 26, @"\d{22}", true, "IS140159260076545510730339") },
            { "IT", new IbanHelper("IT", 27, @"[A-Z]\d{10}[a-zA-Z0-9]{12}", true, "IT60X0542811101000000123456") },
            { "LB", new IbanHelper("LB", 28, @"\d{4}[a-zA-Z0-9]{20}", false, "LB62099900000001001901229114") },
            { "LI", new IbanHelper("LI", 21, @"\d{5}[a-zA-Z0-9]{12}", true, "LI21088100002324013AA") },
            { "LT", new IbanHelper("LT", 20, @"\d{16}", true, "LT121000011101001000") },
            { "LU", new IbanHelper("LU", 20, @"\d{3}[a-zA-Z0-9]{13}", true, "LU280019400644750000") },
            { "LV", new IbanHelper("LV", 21, @"[A-Z]{4}[a-zA-Z0-9]{13}", true, "LV80BANK0000435195001") },
            { "MC", new IbanHelper("MC", 27, @"\d{10}[a-zA-Z0-9]{11}\d\d", true, "MC1112739000700011111000h79") },
            { "ME", new IbanHelper("ME", 22, @"\d{18}", false, "ME25505000012345678951") },
            { "MK", new IbanHelper("MK", 19, @"\d{3}[a-zA-Z0-9]{10}\d\d", false, "MK07300000000042425") },
            { "MT", new IbanHelper("MT", 31, @"[A-Z]{4}\d{5}[a-zA-Z0-9]{18}", true, "MT84MALT011000012345MTLCAST001S") },
            { "MU", new IbanHelper("MU", 30, @"[A-Z]{4}\d{19}[A-Z]{3}", false, "MU17BOMM0101101030300200000MUR") },
            { "NL", new IbanHelper("NL", 18, @"[A-Z]{4}\d{10}", true, "NL91ABNA0417164300") },
            { "NO", new IbanHelper("NO", 15, @"\d{11}", true, "NO9386011117947") },
            { "PL", new IbanHelper("PL", 28, @"\d{8}[a-zA-Z0-9]{16}", true, "PL27114020040000300201355387") },
            { "PT", new IbanHelper("PT", 25, @"\d{21}", true, "PT50000201231234567890154") },
            { "RO", new IbanHelper("RO", 24, @"[A-Z]{4}[a-zA-Z0-9]{16}", true, "RO49AAAA1B31007593840000") },
            { "RS", new IbanHelper("RS", 22, @"\d{18}", false, "RS35260005601001611379") },
            { "SA", new IbanHelper("SA", 24, @"\d{2}[a-zA-Z0-9]{18}", false, "SA0380000000608010167519") },
            { "SE", new IbanHelper("SE", 24, @"\d{20}", true, "SE4550000000058398257466") },
            { "SI", new IbanHelper("SI", 19, @"\d{15}", true, "SI56191000000123438") },
            { "SK", new IbanHelper("SK", 24, @"\d{20}", true, "SK3112000000198742637541") },
            { "SM", new IbanHelper("SM", 27, @"[A-Z]\d{10}[a-zA-Z0-9]{12}", false, "SM86U0322509800000000270100") },
            { "TN", new IbanHelper("TN", 24, @"\d{20}", false, "TN5914207207100707129648") },
            { "TR", new IbanHelper("TR", 26, @"\d{5}[a-zA-Z0-9]{17}", false, "TR330006100519786457841326") }
        };

        /// <summary>
        /// Main method that checks if the supplied IBAN code is valid
        /// </summary>
        /// <typeparam name="iban">IBAN code to be checked</typeparam>
        /// <typeparam name="cleanText">If your IBAN code contains white space and/or not all capital you can pass true to this method. 
        /// If true it will clear and capitalize the supplied IBAN number</typeparam>
        public static IbanStatus Check(string iban, bool cleanText = true)
        {
            if (iban == null)
            {
                return new IbanStatus(ValidationMessages.Valid, true);
            }

            try
            {
                if (cleanText)
                {
                    iban = Regex.Replace(iban, @"\s", string.Empty)
                        .ToUpper(); // remove empty space & convert all uppercase       
                }

                if (Regex.IsMatch(iban, @"\W")) // contains chars other than (a-zA-Z0-9)
                {
                    return new IbanStatus(ValidationMessages.IllegalCharactersFound);
                }

                if (!Regex.IsMatch(iban, @"^\D\D\d\d.+"))
                {
                    // first chars are letter letter digit digit
                    return new IbanStatus(ValidationMessages.WrongStructure);
                }

                if (Regex.IsMatch(iban, @"^\D\D00.+|^\D\D01.+|^\D\D99.+"))
                {
                    // check digit are 00 or 01 or 99
                    return new IbanStatus(ValidationMessages.WrongCheckDigits);
                }

                var countryCode = iban.Substring(0, 2);

                var currentIbanData = IbanCountryValidations[countryCode];

                if (currentIbanData == null)
                {
                    // test if country respected
                    return new IbanStatus(string.Format(ValidationMessages.CountryIbanNotDefined, countryCode));
                }

                if (iban.Length != currentIbanData.Length)
                { // fits length to country
                    return new IbanStatus(string.Format(ValidationMessages.CountryWrongLength, countryCode,
                        currentIbanData.Length));
                }

                if (!Regex.IsMatch(iban.Remove(0, 4), currentIbanData.Regex))
                {
                    // check country specific structure
                    return new IbanStatus(ValidationMessages.CountryWrongStructure);
                }

                // ******* from wikipedia.org
                // The checksum is a basic ISO 7064 mod 97-10 calculation where the remainder must equal 1.
                // To validate the checksum:
                // 1- Check that the total IBAN length is correct as per the country. If not, the IBAN is invalid. 
                // 2- Move the four initial characters to the end of the string. 
                // 3- Replace each letter in the string with two digits, thereby expanding the string, where A=10, B=11, ..., Z=35. 
                // 4- Interpret the string as a decimal integer and compute the remainder of that number on division by 97. 
                // The IBAN number can only be valid if the remainder is 1.
                var modifiedIban = iban.ToUpper().Substring(4) + iban.Substring(0, 4);

                modifiedIban = Regex.Replace(modifiedIban, @"\D", m => (m.Value[0] - 55).ToString());

                var remainer = 0;

                while (modifiedIban.Length >= 7)
                {
                    remainer = int.Parse(remainer + modifiedIban.Substring(0, 7)) % 97;
                    modifiedIban = modifiedIban.Substring(7);
                }

                remainer = int.Parse(remainer + modifiedIban) % 97;

                if (remainer != 1)
                {
                    return new IbanStatus(ValidationMessages.Invalid);
                }

                return new IbanStatus(ValidationMessages.Valid, true);
            }
            catch (Exception)
            {
                return new IbanStatus($"(exception) {ValidationMessages.Invalid}");
            }
        }
    }
}