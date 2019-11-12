import CommentModel from './comment-model';

export default interface PostModel{
    postID:number,
    userID:number,
    text:string,
    timeSent:Date,
    comments:CommentModel[]
};