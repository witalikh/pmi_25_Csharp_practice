using System.Reflection;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace practice_task_1
{
    class ContractValidators
    {
        public static bool ValidateName(string name)
        {
            Regex rgx = new(@"^[A-Z][a-z]{1,16}$");
            return rgx.IsMatch(name);
        }

        public static bool ValidateEmail(string email)
        {
            Regex rgx = new(@"^[a-z0-9.]+@[a-z0-9.]+.[a-z0-9.]+");
            return rgx.IsMatch(email);
        }

        public static bool ValidatePhoneNumberUA(string phoneNumberUA)
        {
            Regex rgx = new(@"\+380[0-9]{9}");
            return rgx.IsMatch(phoneNumberUA);
        }

        public static bool ValidateIBAN(string IBAN)
        {
            Regex rgx = new(@"^[a-zA-Z]{2}[0-9]{2}\s?[a-zA-Z0-9]{4}\s?[0-9]{4}\s?[0-9]{3}([a-zA-Z0-9]\s?[a-zA-Z0-9]{0,4}\s?[a-zA-Z0-9]{0,4}\s?[a-zA-Z0-9]{0,4}\s?[a-zA-Z0-9]{0,3})?$");
            return rgx.IsMatch(IBAN);            
        }
    }
    class Helpers
    {
        public static object? ParseFromString(Type type, string to_parse)
        {
            if (type == typeof(string))
            {
                return to_parse;
            }
            else if (type == typeof(int?))
            {
                int value;
                if (!int.TryParse(to_parse, out value))
                    return null;
                return value;
            }
            else if (type == typeof(DateOnly?))
            {
                DateOnly value;
                if (!DateOnly.TryParse(to_parse, out value))
                    return null;
                return value;
            }
            else
            {
                return null;
            }
        }
    }

    class Contract : IRecognizable<string>,
        IValidatable,
        IFullyModifiable<IReadOnlyDictionary<string, string>>,
        ILookupAble<string>
    {
        private int? _ID;
        private string? _contractorFirstName;
        private string? _contractorLastName;
        private string? _contractorEmail;
        private string? _contractorPhoneNumber;
        private string? _contractorIBAN;
        private DateOnly? _startDate;
        private DateOnly? _endDate;
        private Dictionary<string, string> _errors = new();

        private static readonly ReadOnlyCollection<string> _keyProperties = new(
            new[]
            {
                "ID",
                "ContractorFirstName",
                "ContractorLastName",
                "ContractorEmail",
                "ContractorPhoneNumber",
                "ContractorIBAN",
                "StartDate",
                "EndDate"
            });

        public Contract()
        {
        }

        public Contract(
            int? ID,
            string? contractorFirstName,
            string? contractorLastName,
            string? contractorEmail,
            string? contractorPhoneNumber,
            string? contractorIBAN,
            DateOnly? startDate,
            DateOnly? endDate)
        {
            this.ID = ID;
            ContractorFirstName = contractorFirstName;
            ContractorLastName = contractorLastName;
            ContractorEmail = contractorEmail;
            ContractorPhoneNumber = contractorPhoneNumber;
            ContractorIBAN = contractorIBAN;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Contract(Dictionary<string, string> data)
        {
            ParseData(data);
        }

        public void ParseData(Dictionary<string, string> data, bool partialEdit = false)
        {
            foreach (string propName in KeyProperties)
            {
                PropertyInfo? p = GetType().GetProperty(propName);

                if (p == null)
                    continue;

                string? inputData;
                Type type = p.PropertyType;
                data.TryGetValue(p.Name, out inputData);

                if (inputData == null)
                {
                    if (!partialEdit)
                        _errors.Add(
                            p.Name,
                            "Read error. No data provided for the field");
                    continue;
                }

                object? parsedData = Helpers.ParseFromString(type, inputData);

                if (parsedData == null)
                {
                    _errors.Add(
                        p.Name,
                        String.Format(
                            "Parsing error. Input string does not respresent a {0} type",
                            p.PropertyType.ToString()));
                    continue;
                }

                p.SetValue(
                    this,
                    parsedData);
            }
        }

        public int? ID
        {
            get => _ID;
            set
            {
                if (value == null)
                    return;
                if (value > 0)
                {
                    _ID = value;
                }
                else
                {
                    _errors.Add("ID", "Logic error: ID must be a positive value");
                }
            }
        }

        public string? ContractorFirstName
        {
            get => _contractorFirstName;
            set
            {
                if (value == null)
                    return;
                if (ContractValidators.ValidateName(value))
                {
                    _contractorFirstName = value;
                }
                else
                {
                    _errors.Add(
                        "ContractorFirstName",
                        "Formatting error: Name must start with capital letter and container only alphabetical values");
                }
            }
        }

        public string? ContractorLastName
        {
            get => _contractorLastName;
            set
            {
                if (value == null)
                    return;
                if (ContractValidators.ValidateName(value))
                {
                    _contractorLastName = value;
                }
                else
                {
                    _errors.Add(
                        "ContractorLastName",
                        "Formatting error: Surname must start with capital letter and container only alphabetical values");
                }
            }
        }

        public string? ContractorEmail
        {
            get => _contractorEmail;
            set
            {
                if (value == null)
                    return;
                if (ContractValidators.ValidateEmail(value))
                {
                    _contractorEmail = value;
                }
                else
                {
                    _errors.Add(
                        "ContractorEmail",
                        "Formatting error: Wrong email format");
                }
            }
        }

        public string? ContractorPhoneNumber
        {
            get => _contractorPhoneNumber;
            set
            {
                if (value == null)
                    return;
                if (ContractValidators.ValidatePhoneNumberUA(value))
                {
                    _contractorPhoneNumber = value;
                }
                else
                {
                    _errors.Add(
                        "ContractorPhoneNumber",
                        "Formatting error: Wrong ukrainian number format");
                }
            }
        }

        public string? ContractorIBAN
        {
            get => _contractorIBAN;
            set
            {
                if (value == null)
                    return;
                if (ContractValidators.ValidateIBAN(value))
                {
                    _contractorIBAN = value;
                }
                else
                {
                    _errors.Add(
                        "ContractorIBAN",
                        "Formatting error: Wrong IBAN format");
                }
            }
        }

        public DateOnly? StartDate
        {
            get => _startDate;
            set { _startDate = value; }
        }

        public DateOnly? EndDate
        {
            get => _endDate;
            set { _endDate = value; }
        }

        public static ReadOnlyCollection<string> KeyProperties
        {
            get => _keyProperties;
        }

        private bool ValidateLogic()
        {
            if (StartDate == null && EndDate == null)
                return false;
            if (StartDate > EndDate)
            {
                _errors.Add(
                    "StartDate",
                    "Start date can not be earlier than end date");
                return false;
            }

            return true;
        }

        public Dictionary<string, string> GetErrors()
        {
            ValidateLogic();
            return _errors;
        }
        
        public Dictionary<string, string> ToDictionary()
        {
            var serializedInstance = new Dictionary<string, string>();

            foreach (var pName in KeyProperties)
            {
                var p = GetType().GetProperty(pName);
                var value = p!.GetValue(this);
                if (value is null)
                    continue;
                string valueRepr = value.ToString() ?? "";
                serializedInstance.Add(pName, valueRepr);
            }

            return serializedInstance;
        }
        
        public override string ToString()
        {
            string repr = "";
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                repr += prop.Name + ": " + Convert.ToString(prop.GetValue(this, null));
                repr += "\n";
            }

            return repr;
        }

        public void Modify(IReadOnlyDictionary<string, string> varName) => 
            ParseData((Dictionary<string, string>) varName);

        public bool Contains(string lookupExpr)
        {
            return ToString().Contains(lookupExpr);
        }

        public string[] Keys() => KeyProperties.ToArray();

        public Dictionary<string, string> Items() => ToDictionary();

        public ErrorsDict GetValidationErrors()
        {
            var errorsDict = new ErrorsDict();
            foreach ((string key, string value) in _errors)
            {
                errorsDict.Add(key, value);
            }

            return errorsDict;
        }

        public string? Id => Convert.ToString(_ID);
    }
}