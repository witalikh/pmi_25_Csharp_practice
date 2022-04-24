namespace practice_task_1.static_files;

static class Languages
{
    public static Dictionary<string, string> English = new()
    {
        {
            "StaffMenu", 
            "Available choices:\n" +
            " 0. Print available options,\n" +
            " -. ---------------\n" +
            " 1. Print all public certificates,\n" +
            " 2. Print certificates public containing value,\n" +
            " -. ---------------\n" +
            " 3. Print all own certificates,\n" +
            " 4. Filter all own certificates,\n" +
            " -. ---------------\n" +
            " 5. Add new certificate (into draft),\n" +
            " 6. Edit certificate (into draft, including rejected),\n" +
            " 7. Remove certificate (into draft, including rejected),\n" +
            " -. ---------------\n" +
            " 8. Set sort,\n" +
            " -. ---------------\n" +
            " 9. Logout,\n" +
            " --. ---------------\n" +
            "Type \"exit\", \"-1\" or \"quit\" (any case) to exit."
        },
        
        {
            "AdminMenu", 
            "Available choices:\n" +
            " 0. Print available options,\n" +
            " -. ---------------\n" +
            " 1. Print all users,\n" +
            " -. ---------------\n" +
            " 2. Print all approved certificates,\n" +
            " 3. Print all draft certificates,\n" +
            " -. ---------------\n" +
            " 4. Filter and print all approved certificates,\n" +
            " 5. Filter and print all draft certificates,\n" +
            " -. ---------------\n" +
            " 6. Approve a draft,\n" +
            " 7. Reject a draft,\n" +
            " -. ---------------\n" +
            " 8. Add new certificate,\n" +
            " 9. Edit certificate,\n" +
            " 10. Remove certificate,\n" +
            " --. ---------------\n" +
            " 11. Set sort,\n" +
            " --. ---------------\n" +
            " 12. Logout,\n" +
            " --. ---------------\n" +
            "Type \"exit\", \"-1\" or \"quit\" (any case) to exit."
        },
        
        {
            "AnonymousMenu", 
            "Available choices:\n" +
            " 0. Print available options,\n" +
            " -. -------\n" +
            " 1. Login,\n" +
            " 2. Sign-up,\n" +
            " -. -------\n" +
            "Type \"exit\", \"-1\" or \"quit\" (any case) to exit."
        },

        {
            "ChooseFieldMenu", " 0. Certificate ID,\n" +
                               " 1. Username,\n" +
                               " 2. International passport,\n" +
                               " 3. Start date,\n" +
                               " 4. End date,\n" +
                               " 5. Date of birth,\n" +
                               " 6. Vaccine."
        },
        
        {"Email", "Email: "},
        {"FirstName", "First name: "},
        {"LastName", "Last name: "},
        {"FirstDayInCompany", "First day in company: "},
        {"Salary", "Salary: "},
        
        {"PasswordInit", "Password: "},
        {"PasswordConfirm", "Password (again): "},
        {"PasswordLogin", "Password: "},
        
        {"AuthenticationFailed", "Login failed: user with given credentials doesn't exist!"},
        {"AuthenticationSuccess", "You have entered into system!"},

        {"SignUpSuccess", "New user has been added!"},


        {"ChooseFieldOption", "> "},
        {"ChooseOption", ">>> "},
        {"WrongOption", ""},
        {"WrongField", "This field does not exist. You must have mistyped."},
        {"WrongQuery", "Command not found"},

        // {"OpenedFile", "Opened file is {0}"},
        // {"NotOpenedFile", "No file is chosen."},
        // {"FileOpenRequest", "Enter the file path here"},
        // {"FileNotSpecified", "The action cannot be performed unless the file is specified!"},
        // {"FileNotExistWarning", "Specified file did not exist, so we created it for you!"},
        // {"AlreadyClosed", "File was already closed!"},
        // {"LoadErrorsFound", "There were some errors in fields in the file, backup file is written!"},
        // {"FileCorruptedError", "File data can't be read, there are some errors!"},

        // {"SuccessLoad", "Container data was successfully loaded locally!"},
        // {"SuccessDump", "Local container data was successfully dumped into file!"},

        {"SuccessAdd", "New certificate was successfully added!"},
        {"SuccessEdit", "Certificate was successfully patched!"},
        {"SuccessDelete", "Certificate was successfully removed!"},

        {"SuccessSort", "Entries were successfully sorted!"},
        {"SuccessClean", "Local container was cleaned!"},

        {"AlreadyClean", "Local container is already clean!"},

        {"EnterId", "Enter certificate ID: "},
        {"IdCollision", "Certificate with this id already exists!"},
        {"IdAbsent", "Certificate with this id does not exist!"},
        {"IdViolation", "Certificate ID is not changeable!"},

        {"EnterFilterValue", "Enter the value you are looking for: "},

        {"PrintAllPrefix", "The list of all certificates: "},
        {"PrintFilteredPrefix", "The list of certificates with specified value {0}"},
        
        {"PrintAllDraftPrefix", "The list of all certificates in draft: "},
        {"PrintFilteredDraftPrefix", "The list of draft certificates with specified value {0}"},

        {"Size", "Found entries: {0}"},

        {"Id", "Certificate's id: "},
        {"Passport", "International passport code: "},
        {"Username", "Username: "},
        {"StartDate", "Start date: "},
        {"EndDate", "End date: "},
        {"BirthDate", "Date of birth: "},
        {"Vaccine", "Vaccine: "},

        {"IdFormatError", "Certificate's id is empty or invalid"},
        {"PassportFormatError", "Passport code is empty or does not follow the pattern 'AA123456'"},
        {"UsernameFormatError", "Username cannot be empty, contain digits or other symbols"},
        {"StartDateFormatError", "Start date is empty, has invalid format or this date does not exist"},
        {"EndDateFormatError", "End date is empty, has invalid format or this date does not exist"},
        {"BirthDateFormatError", "Date of birth is empty, has invalid format or this date does not exist"},
        {"VaccineFormatError", "This vaccine is not listed among allowed"},

        {"StartDateEndDateSequenceError", "Start date cannot exceed end date!"},
        {"BirthDateStartDateSequenceError", "Date of birth cannot exceed start date!"},
        {"BirthDateEndDateSequenceError", "Date of birth cannot exceed end date!"},

        {"BirthDateTooEarly", "The age is under 14, not permitted"},
        {"BirthDateTooLate", "The age does not seem to be exceed 125 years ever"},

        {"StartDateTooEarly", "The person cannot be vaccinated in that date (too young!)"},
        {"StartDateTooLate", "The certificate cannot be issued later than two weeks!"}
    };
}