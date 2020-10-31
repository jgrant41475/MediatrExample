import { Component, OnInit } from '@angular/core';

import * as $ from 'jquery';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
      console.log("test")
      $(document).ready(function (){
          console.log("Loading...");
          $.get("https://localhost:5001/customers").done(function(res){
              console.log("Loaded.");
              console.log(res);
          }).fail(function(){
              console.error("GET failed");
          });
      });
  }

}
