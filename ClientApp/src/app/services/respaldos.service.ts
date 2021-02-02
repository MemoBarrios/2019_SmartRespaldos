import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpEventType, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IRespaldo } from '../home/IRespaldo';
import { ISucursal } from '../home/ISucursal';
import { IServidor } from '../Interfaces/IServidor';

@Injectable({
  providedIn: 'root'
})
export class RespaldosService {

  private apiURL = this.baseUrl + "Respaldos";

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAllSucursales(): Observable<ISucursal[]> {
    return this.http.get<ISucursal[]>(this.apiURL + "/GetSucursales");
  }

  getJobsResp(sucursales: any, fecha: string, tipoConsulta: string): Observable<HttpEvent<any>> {
    return this.http.get(this.apiURL + "/GetJobsRespaldos/", {
      responseType: "json", reportProgress: true, observe: "events", headers: new HttpHeaders(
        { 'Content-Type': 'application/json' },
      ),
      params: new HttpParams().set("sucursales", sucursales).set("fecha", fecha).set("tipoConsulta", tipoConsulta)
    });
  }

  getRutasResp(sucursales: any): Observable<HttpEvent<IServidor[]>> {
    return this.http.get<IServidor[]>(this.apiURL + "/GetRutasRespaldos/", {
      reportProgress: true, observe: "events", headers: new HttpHeaders(
        { 'Content-Type': 'application/json' },
      ),
      params: new HttpParams().set("sucursales", sucursales)
    });
  }
}
