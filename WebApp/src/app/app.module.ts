import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {ProductsComponent} from './products/products.component';

import {HttpClientModule, HttpClient} from "@angular/common/http";
import { CustomersComponent } from './customers/customers.component';
import { ProductOrdersComponent } from './product-orders/product-orders.component';
import { CustomerDialogComponent } from './customers/customer-dialog/customer-dialog.component';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatDialogModule} from "@angular/material/dialog";
import {Overlay} from "@angular/cdk/overlay";

@NgModule({
    declarations: [
        AppComponent,
        ProductsComponent,
        CustomersComponent,
        ProductOrdersComponent,
        CustomerDialogComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
    ],
    exports: [
        MatDialogModule,
        BrowserAnimationsModule,
    ],
    providers: [
        HttpClientModule,
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
