import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Product } from './models/product';
import { Pagination } from './models/pagination';

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
		this.http.get<Pagination<Product[]>>('https://localhost:5001/api/products').subscribe({
			next: (response) => (this.products = response.data),
			complete: () => {
				console.log('request completed');
			},
		});
	}
}
function Pagination<T>(arg0: string) {
	throw new Error('Function not implemented.');
}
