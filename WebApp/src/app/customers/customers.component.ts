import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {MatDialog} from "@angular/material/dialog";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {CustomerDialogComponent} from './customer-dialog/customer-dialog.component';

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

    constructor(private http: HttpClient, public dialog: MatDialog) {
    }

    ngOnInit(): void {
        this.customers$ = this.http.get<Customer[]>("https://localhost:5001/customers");
    }

    openDialog() {
        const dialogRef = this.dialog.open(CustomerDialogComponent);

        dialogRef.afterClosed().subscribe(result => {
            console.log(`Dialog result: ${result}`);
        })
    }
}
