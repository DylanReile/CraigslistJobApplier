Automatically applies to job postings on Craigslist.

- Command Line Usage

  -c, --craigslistUrl           Required. Craigslist URL for the target
                                location and job category. EX:
                                http://nyc.craigslist.org/search/sof

  -a, --gmailAddress            Required. Address of the Gmail account used to
                                send emails. EX: bob@gmail.com

  -p, --gmailPassword           Required. Password to the Gmail account.

  -m, --messageFile             Required. File containing the email message.

  -r, --resumeFile              Required. Resume file that will be attached to
                                the email.

  -o, --sentEmailsOutputFile    Required. File used to log email addresses that
                                have already received emails. Used to avoid
                                sending duplicates.

  -s, --secondsBetweenEmails    Required. Seconds to wait between emails. Used
                                to avoid Craigslist spam filters.

  --help                        Display this help screen


Example usage:
CraigslistJobApplier.exe -c http://nyc.craigslist.org/search/sof -a bob@gmail.com -p hunter2 -m C:\Users\bob\Documents\applicationBlurb.txt -r C:\Users\bob\Documents\BobResume.pdf -o emailsLog.txt -s 60