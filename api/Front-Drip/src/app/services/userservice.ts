import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {firstValueFrom, Observable} from 'rxjs';
import {Product, User, UserCartItems, UserLikesItem} from "../models";
import {jwtDecode} from "jwt-decode";
import {environment} from "../../environments/environment.prod";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  users: User[] = [];
  user: any
  currentUser: User;
  userCartItems: UserCartItems[] = [];
  userLikedItems: UserLikesItem[] = [];

  constructor(private http: HttpClient) {
    const token = localStorage.getItem('jwtToken')
    if(token){
      const decodedToken = jwtDecode(token);
      const expirationDate = new Date(decodedToken.exp! * 1000);
      if(new Date() < expirationDate)
        this.user = decodedToken;
      console.log(this.user)
    }

    this.currentUser = {
      userId: this.user.id,
      userEmail: this.user.email,
      isAdmin: this.user.isadmin
    };
  }

  // A method to get all the users from the server
  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>('api/users');
  }

  // A method to get a user by id from the server
  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`api/users/${id}`);
  }

  // A method to update a user on the server
  updateUser(user: User): Observable<User> {
    return this.http.put<User>(`api/users/${user.userId}`, user);
  }

  // A method to delete a user on the server
  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`api/users/${id}`);
  }

  // A method to set the current user
  setCurrentUser(user: User): void {
    this.currentUser = user;
  }

  // A method to get the current user
  getCurrentUser(): User | null {
    return this.currentUser;
  }

  // A method to add a product to the user cart
  async addToCart(product: UserCartItems){
    const call = this.http.post<any>(environment.baseUrl+'/AddProductToUserCart', {userId: this.currentUser?.userId,  productId: product.productId, sizeId: product.sizeId, colorId: product.colorId, quantity: product.quantity });
    const result= await firstValueFrom<UserCartItems>(call);
    this.userCartItems.push(result);
  }

  // A method to remove a product from the user cart
  async removeFromCart(product: UserCartItems) {
    const call = this.http.delete<string>(environment.baseUrl+`/api/UserCart/RemoveProductFromCart?productId=${product.productId} &colorId=${product.colorId} &sizeId=${product.sizeId} &quantity=${product.quantity}`)
    const result= await firstValueFrom<string>(call);
    if (result==='Product removed from cart successfully.'){
      const index = this.userCartItems.indexOf(product);
      if (index > -1) {
        this.userCartItems.splice(index, 1);
      }
    }
  }

  // A method to get the user cart items
  async getCartItems() {
    const call = this.http.get<UserCartItems[]>(environment.baseUrl+`/GetUserCart`);
    const result = await firstValueFrom<UserCartItems[]>(call);
    this.userCartItems = result;
    console.log(this.userCartItems)
  }

  // A method to add a product to the user liked items
  async addToLiked(product: UserLikesItem){
    const call= this.http.post<any>(environment.baseUrl+'/AddProductToUserLikes', { userId: this.currentUser?.userId, productId: product.productId });
    const result = await firstValueFrom<UserLikesItem>(call);
    this.userLikedItems.push(result)
  }

  // A method to remove a product from the user liked items
  async removeFromLiked(product: UserLikesItem){
    const call = this.http.delete<string>(environment.baseUrl+`/api/UserCart/RemoveProductFromLiked?productId=${product.productId}`);
    const result = await firstValueFrom<string>(call);
    if (result==='Product removed from likes succesfully'){
      const index = this.userLikedItems.indexOf(product)
      if (index>-1){
        this.userLikedItems.splice(index,1)
      }
    }
  }

  // A method to get the user liked items
  async getLikedItems(): Promise<void> {
    const call= this.http.get<UserLikesItem[]>(environment.baseUrl+'/api/GetUserLikes');
    const result = await firstValueFrom<UserLikesItem[]>(call);
    this.userLikedItems = result;
  }
}
