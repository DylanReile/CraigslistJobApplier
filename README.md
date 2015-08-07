### Command Line Usage

  -c, --craigslistUrl           Required. Craigslist URL for the target
                                location and job category. EX:
                                http://nyc.craigslist.org/search/sof

  -g, --gmailAddress            Required. Address of the Gmail account used to
                                send emails. EX: bob@gmail.com

  -p, --gmailPassword           Required. Password to the Gmail account.

  -m, --messageFile             Required. File containing the email message.

  -a, --attachments             Files that will be attached to the email. Separated by ',' without spaces. EX:resume.pdf,coverLetter.pdf

  -o, --sentEmailsOutputFile    Required. File used to log email addresses that
                                have already received emails. Used to avoid
                                sending duplicates.

  -s, --secondsBetweenEmails    <Default: 5> Seconds to wait between emails. Used
                                to avoid Craigslist spam filters.

  --help                        Display this help screen


Example usage:
CraigslistJobApplier.exe -c http://nyc.craigslist.org/search/sof -g bob@gmail.com -p hunter2 -m applicationBlurb.txt -a resume.pdf,coverLetter.pdf -o emailsLog.txt