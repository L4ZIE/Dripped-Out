import {Component, OnInit} from "@angular/core";
import { ProductService } from 'ProductService.cs';
import { ActivatedRoute} from "@angular/router";

@Component({
  selector: 'product-details',
  templateUrl: 'product-details-page.component.html',
  styleUrls:['product-details-page.component.css']
})

export class ProductDetailsPageComponent implements OnInit{
product:any

  constructor(
    private productService: ProductService,
    private route: ActivatedRoute
  ) {
  }
  ngOnInit(): void {
    const productId = this.route.snapshot.params['id']

    this.productService.getProductById(productId).subscribe((data) => {
      this.product =data;
    });
  }
}
