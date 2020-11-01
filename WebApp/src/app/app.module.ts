import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {ProductsComponent} from './products/products.component';

import {HttpClientModule, HttpClient} from "@angular/common/http";
import { CustomersComponent } from './customers/customers.component';
import { ProductOrdersComponent } from './product-orders/product-orders.component';

@NgModule({
    declarations: [
        AppComponent,
        ProductsComponent,
        CustomersComponent,
        ProductOrdersComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
    ],
    providers: [
        HttpClientModule
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
