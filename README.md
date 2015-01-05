# Mail REST API

## Overview

This is a REST API to send email. This was initially created as an alternative way to send email.
The website is currently hosted in Amazon EC2 (54.148.60.59) and setup as a virtual application under
aws.filgifts.com. The url is aws.filgifts.com/mail

The application consist of 3 parts:
1. MailData - Database read/write
2. MailRest - Web Service API that receives mail request
3. MailSender - Standalone console application that sends the mail


## MailData

Database - FilgiftsMail

Has two tables
1. MailDetails - contains all the mails
2. MailError - contains details of error



