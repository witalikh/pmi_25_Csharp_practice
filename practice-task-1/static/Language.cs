﻿namespace practice_task_1.static_files;

internal static class Languages
{
    public static readonly Dictionary<string, string> English = new()
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
            " 1a. Change salary for a staff member,\n" +
            " 1b. Fire staff member,\n" +
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
        {
            "ChooseType", " - 0. Approved,\n" +
                          " - 1. Draft,\n" +
                          " - 2. Rejected,\n" +
                          " - Anything else - all."
        },
        
        {"Email", "Email: "},
        {"FirstName", "First name: "},
        {"LastName", "Last name: "},
        {"FirstDayInCompany", "First day in company: "},
        {"Salary", "Salary: "},
        
        {"EmailFormat", "Invalid email address"},
        {"FirstNameFormat", "First name should contain at least 2 letters and only letters"},
        {"LastNameFormat", "Last name should contain at least 2 letters and only letters"},
        {"PasswordFormat", "Password should contain at least one capital, one small and one digit"},
        {"PasswordMismatch", "Passwords do not match"},

        {"PasswordInit", "Password: "},
        {"PasswordConfirm", "Password (again): "},
        {"PasswordLogin", "Password: "},
        
        {"AuthenticationFailed", "Login failed: user with given credentials doesn't exist!"},
        {"AuthenticationSuccess", "You have entered into system!"},

        {"SignUpSuccess", "New user has been added!"},
        
        {"SuccessApprove", "Certificate was successfully approved from draft!"},
        {"SuccessReject", "Certificate was rejected!"},
        {"RejectComment", "Enter the reason of reject: "},
        {"NotInDraft", "Current certificate is not in draft!"},
        
        {"Draft", "Draft status: {0}"},
        {"Comment", "Comment: {0}"},

        {"ChooseFieldOption", "> "},
        {"ChooseOption", ">>> "},
        {"WrongOption", ""},
        {"WrongField", "This field does not exist. You must have mistyped."},
        {"WrongQuery", "Command not found"},

        {"StaffEmail", "Enter staff email here: "},
        {"AbsentUser", "User doesn't exist!"},
        {"NotStaffUser", "User is not staff!"},
        
        {"InvalidSalary", "Salary value is invalid! "},
        {"SuccessSalaryEdit", "Salary was changed!"},
        {"SuccessFire", "Staff member was successfully fired!"},

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