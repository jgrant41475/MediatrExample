import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

export interface Product {
    id: string,
    name: string,
    description: string,
    price: number,
    isAnimal: boolean,
};

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
    products$: Observable<Product[]>;

    constructor(private http: HttpClient) {
    }

    ngOnInit(): void {
        this.products$ = this.http.get<Product[]>("https://localhost:5001/products");
    }
}
