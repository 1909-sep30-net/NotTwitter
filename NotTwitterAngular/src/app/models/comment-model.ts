import CommentCreate from './comment-create-model';

export default interface CommentModel extends CommentCreate{
    commentId:number,
    timeSent:Date
};