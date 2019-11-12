import FriendModel from './friend-model';

export default interface UserModel{
    id: number,
    firstName: string,
    lastName: string,
    username: string,
    password: string,
    email: string,
    gender: number,
    friends: FriendModel[]
};