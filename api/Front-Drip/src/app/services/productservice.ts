import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {firstValueFrom, Observable} from 'rxjs';
import {Product, ProductImageDto, ProductImageFile} from "../models";

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  //List of products to be displayed
  products: Product[] = [];
  productImageFiles: ProductImageFile[] = [];
  // The current selected product
  currentProduct: Product | null = null;

  constructor(private http: HttpClient) {

  }

  // A method to get all the products from the server
  async getAllProductsFromServer() {
    const call = this.http.get<Product[]>('http://localhost:5027/api/GetAllProducts');
    const result = await firstValueFrom<Product[]>(call);
    this.products = result;
  }


  // A method to get a product by id from the server
  async getProductById(id: number) {
    const call = this.http.get<Product>(`http://localhost:5027/api/product/GetProductById?productId=` + id);
    const result = await firstValueFrom<Product>(call);
    this.currentProduct = result;
  }

  // A method to update a product on the server
  updateProduct(product: Product): Observable<Product> {
    return this.http.put<Product>(`api/products/${product.productId}`, product);
  }

  // A method to delete a product on the server
  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`api/products/${id}`);
  }

  getProductList(): Product[]{
    return this.products
  }

  async createNewProduct(name: string, productType: string, colors: string[], colorFiles: File[], productPrice: number, productGender: string, productDescription: string){
    let type: number = this.getTypeIdFromString(productType);
    let colorIds: number[] = [];
    //create new product and add it to database, retrieve it again to get new productId
    let product: Product = {
      productId: 0,
      productName: name,
      typeId: type,
      price: productPrice,
      gender: productGender,
      description: productDescription
    }
    const productCall = this.http.post<Product>('http://localhost:5027/api/product/createProduct', product)
    const productResult = await firstValueFrom<Product>(productCall);

    for (let i = 0; i< colors.length; i++){
      //upload image and get blob uri
      let formData = new FormData();
      formData.append('file', colorFiles[i]);
      const blobCall = this.http.post('http://localhost:5027/uploadfile', formData, {responseType: 'text'});
      const blobResult = await firstValueFrom(blobCall);

      //upload productimage to database
      const prodImageCall = this.http.post<ProductImageDto>('http://localhost:5027/api/product-image-create', {productId: productResult.productId, colorId: this.getColorIdFromString(colors[i]), blobUrl: blobResult});
      const prodImageResult = await firstValueFrom<ProductImageDto>(prodImageCall);

      //add color to list of colorids
      colorIds.push(this.getColorIdFromString(colors[i]));
    }


    const prodColorCall = this.http.put('http://localhost:5027/AddColorsToProduct?productId='+ productResult.productId, colorIds)
  }

  async getProductImages(productId: number){
    let productImageDto: ProductImageDto[] = [];

    //gets list of imageDtos which hold the blob uri to get image file
    const imageDtoCall = this.http.get<ProductImageDto[]>('http://localhost:5027/api/product-image-get-all?productId=' + productId);
    const imageDtoResult = await firstValueFrom<ProductImageDto[]>(imageDtoCall);

    productImageDto = imageDtoResult;

    //clear all images in productImageFiles
    for (let i = 0; i < this.productImageFiles.length; i++) {
      this.productImageFiles.splice(i, 1);
    }
    //uses list of imageDtos to get images from blob storage and create productImageFiles
    for (let i = 0; i<productImageDto.length; i++){
      const imageFileCall = this.http.get('http://localhost:5027/getimage?blobUri=' + productImageDto[i].blobUrl, { responseType: 'blob' });
      const imageBlobResult = await firstValueFrom<Blob>(imageFileCall);

      let productImageFile: ProductImageFile = {
        productId: productImageDto[i].productId,
        colorId: productImageDto[i].colorId,
        imageFile: new File([imageBlobResult], String(productImageDto[i].productId)) // replace 'filename' with the actual filename if available
      };

      //Adds newly created imageFile to list
      this.productImageFiles.push(productImageFile);
      console.log(this.productImageFiles)
    }
  }


  getColorIdFromString(colorType: string){
    let type: number;
    switch(colorType){
      case "Blue":
        type = 1;
        break;
      case "Black":
        type = 2;
        break;
      case "White":
        type = 3;
        break;
      case "Green":
        type = 4;
        break;
      case "Yellow":
        type = 5;
        break;
      case "Red":
        type = 6;
        break;
      case "Purple":
        type = 7;
        break;
      case "Orange":
        type = 8;
        break;
      default:
        throw new Error("Invalid Color Type");
    }
    return type;
  }

  getTypeIdFromString(productType: string){
    let type: number;
    switch (productType) {
      case "Pants":
        type = 1;
        break;
      case "T-Shirt":
        type = 2;
        break;
      case "Long Sleeve":
        type = 3;
        break;
      case "Jacket":
        type = 4;
        break;
      case "Dresses":
        type = 5;
        break;
      case "Shorts":
        type = 6;
        break;
      case "Hats":
        type = 8;
        break;
      case "Jewelery":
        type = 9;
        break;
      default:
        throw new Error("Invalid Product Type");
    }
    return type;
  }


}
