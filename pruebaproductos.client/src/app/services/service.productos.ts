/* eslint-disable no-var */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Producto } from '../models/producto';


@Injectable()
export class ServiceProductos {
  constructor(private _http: HttpClient) { }


  getAllProducts(): Observable<any> {
    return this.llamarApi('', {});
  }


  llamarApi(metodo: string, params: { [key: string]: any }): Observable<Producto[]> {
    const queryParams = new URLSearchParams();
    for (const key in params) {
      if (params[key] !== '') {
        queryParams.set(key, params[key]);
      }
    }
    const fullUrl = `${environment.urlApiProductos}${metodo}?${queryParams.toString()}`;
    return this._http.get<Producto[]>(fullUrl);
  }

  createProducto(producto: Producto): Observable<Producto> {
    return this._http.post<Producto>(environment.urlApiProductos, producto);
  }

  updateProducto(codigo: number, producto: Producto): Observable<Producto> {
    return this._http.put<Producto>(`${environment.urlApiProductos}/${codigo}`, producto);
  }

  deleteProducto(codigo: number): Observable<void> {
    return this._http.delete<void>(`${environment.urlApiProductos}/${codigo}`);
  }


}
