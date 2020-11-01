import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

import {Customer} from "../customers/customers.component";
import {Product} from "../products/products.component";

interface Order {
    id: string,
    orderPlaced: Date,
    customerId: string,
    customer: Customer
}

interface ProductOrder {
    id: string,
    quantity: number,
    productId: string,
    product: Product,
    orderId: string,
    order: Order,
}

@Component({
    selector: 'app-product-orders',
    templateUrl: './product-orders.component.html',
    styleUrls: ['./product-orders.component.scss']
})
export class ProductOrdersComponent implements OnInit {
    productOrders$: Observable<ProductOrder[]>;

    constructor(private http: HttpClient) {
    }

    ngOnInit(): void {
        this.productOrders$ = this.http.get<ProductOrder[]>("https://localhost:5001/ProductOrders/");
    }

}
