import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Product } from './models/product';
import { Pagination } from './models/pagination';
import { environment } from 'src/environments/environment.development';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
	title = 'Skinet';
	products: Product[] = [];

	constructor(private http: HttpClient) {}

	ngOnInit(): void {
		this.http.get<Pagination<Product[]>>(environment.apiUrl + '/api/ProductGrid').subscribe({
			next: (response) => (this.products = response.data),
			complete: () => {
				console.log('request completed');
			},
		});
	}
}
