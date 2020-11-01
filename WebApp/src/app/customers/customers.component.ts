import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

export interface Customer {
    id: string,
    firstName: string,
    lastName: string,
    address: string,
    phone: string,
    email: string,
}

@Component({
    selector: 'app-customers',
    templateUrl: './customers.component.html',
    styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {
    customers$: Observable<Customer[]>;

    constructor(private http: HttpClient) {
    }

    ngOnInit(): void {
        this.customers$ = this.http.get<Customer[]>("https://localhost:5001/customers");
    }

}
