import {Component, inject, Input, OnInit} from "@angular/core";
import {CommonModule} from "@angular/common";
import {Router} from "@angular/router";
import {Product, ProductImageDto, ProductImageFile} from "../models";
import {ProductService} from "../services/productservice";
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment.prod";

@Component({
  selector: 'product-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: 'product-card.component.html',
  styleUrls: ['product-card.component.css']
})

export class ProductCardComponent implements OnInit{

  @Input() product: Product | undefined;

  productImageFiles: ProductImageFile[] = [];

  productImage: ProductImageFile | undefined;
  url: string | undefined;
   constructor(private router: Router, private http: HttpClient) {
  }

  async ngOnInit(){
     await this.getProductImages(<number>this.product?.productId);
    this.productImage = this.productImageFiles[0];

   var reader = new FileReader();
   this.url = await this.readFile(this.productImage.imageFile)
  }
  async getProductImages(productId: number){
    let productImageDto: ProductImageDto[] = [];

    //gets list of imageDtos which hold the blob uri to get image file
    const imageDtoCall = this.http.get<ProductImageDto[]>(environment.baseUrl+'/api/product-image-get-all?productId=' + productId);
    const imageDtoResult = await firstValueFrom<ProductImageDto[]>(imageDtoCall);

    productImageDto = imageDtoResult;

    //clear all images in productImageFiles
    for (let i = 0; i < this.productImageFiles.length; i++) {
      this.productImageFiles.splice(i, 1);
    }

    //uses list of imageDtos to get images from blob storage and create productImageFiles
    for (let i = 0; i<productImageDto.length; i++){
      const imageFileCall = this.http.get(environment.baseUrl +'/getimage?blobUri=' + productImageDto[i].blobUrl, { responseType: 'blob' });
      const imageBlobResult = await firstValueFrom<Blob>(imageFileCall);

      let productImageFile: ProductImageFile = {
        productId: productImageDto[i].productId,
        colorId: productImageDto[i].colorId,
        imageFile: new File([imageBlobResult], String(productImageDto[i].productId)) // replace 'filename' with the actual filename if available
      };

      //Adds newly created imageFile to list
      this.productImageFiles.push(productImageFile);
    }
  }

  readFile(file: File): Promise<any> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = (event: any) => resolve(event.target.result);
      reader.onerror = (event: any) => {
        console.log("File could not be read: " + event.target.error.code);
        reject(event.target.error.code);
      };
      reader.readAsDataURL(file);
    });
  }
  clickProductCard(){
    this.router.navigate(['product-details/' + this.product?.productId])
  }
}
