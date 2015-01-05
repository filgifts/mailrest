# Email Sender

## Overview

This was initially created as an alternative way to send email.
The website is currently hosted in Amazon EC2 (54.148.60.59) and setup as a virtual application under
aws.filgifts.com. The url is aws.filgifts.com/mail.

The system consist of 3 parts:

1. MailData - Database read/write.
2. MailRest - Web Service API that receives mail request.
3. MailSender - Standalone console application that sends the mail.


## MailData

#### Database - FilgiftsMail

Has two tables:

1. MailDetails - contains all the mails.
2. MailError - contains details of error.


## MailRest

Currently configured as a virtual directory ####MailAPI in aws.filgifts.com 

Directory : D:\Web\MailAPI
Url: [http://aws.filgifts.com/MailAPI](http://aws.filgifts.com/MailAPI)

### Parameters
 
1. string From 
2. string To 
3. string Subject
4. string Message - base64 utf-8 format
5. string Cc 
6. string Bcc 
7. bool IsHTML 

### Security

the website is accessible only from the IP remote IP address specified in the ####web.config

```
<add key="whiteListIP" value="::1,127.0.0.1,54.148.60.59,202.95.228.224"/>
```


## MailSender

Directory : D:\Web\MailAPI

The program is set to run in task scheduler every 1 minute.
