export class ComposeMail {
    MailBox_Id: number;
    Fromuser_Id: number;
    Touser_Id: string;
    ToMalId: string;
    CcUser_Id: string;
    Subject: string;
    MailBody: string;
    AttachmentPath: string;
    MailBox_Status: number;
    MailBox_Date: string;
    ParentId: number;
}
export class FavouriteMail {
    MailFavourite_Id: number;
    MailBox_Id: number;
    user_Id: number
    MailFavourite_Status: number;
    MailFavourite_date: string;
}
export class AlertStatus {
    AlertText_Id: number;
    Favourite_Flag: number;
    Alert_CreatedDate: string;
}