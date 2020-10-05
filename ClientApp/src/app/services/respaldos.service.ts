import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpEventType, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IRespaldo } from '../home/IRespaldo';
import { ISucursal } from '../home/ISucursal';

@Injectable({
  providedIn: 'root'
})
export class RespaldosService {

  private apiURL = this.baseUrl + "Respaldos";

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAllRespaldos(ip: string): Observable<IRespaldo[]> {
    return this.http.get<IRespaldo[]>(this.apiURL + "/GetRespaldos/" + ip);
  }

  getAllSucursales(): Observable<ISucursal[]> {
    return this.http.get<ISucursal[]>(this.apiURL + "/GetSucursales");
  }

  //QUEDA PENDIENTE VER PORQUE NO SE PASA LA LISTA DE SUCURSALES AL CONTROLADOR, SE MANDA LA LISTA VACIA.
  getRespaldoSucursales(sucursales: Array<ISucursal>): Observable<Array<ISucursal>> {
    return this.http.get<Array<ISucursal>>(this.apiURL + "/GetRespaldoSucursales/" + sucursales);
  }

  getJobsRespaldos(sucursales: any, fecha: string, tipoConsulta: string): Observable<HttpEvent<any>> {
    //let headers = new HttpHeaders();
    //headers.append('Content-Type', 'application/json');
    //let params = new HttpParams().set("sucursales", JSON.stringify(sucursales));

    //const httpOptions = {
    //  headers: new HttpHeaders({
    //    'Content-Type': 'application/json'
    //  }),
    //  observe: "response",
    //  params: new HttpParams().set("sucursales", sucursales).set("fecha", fecha).set("tipoConsulta", tipoConsulta),
    //  reportProgress: true
    //};
    return this.http.get(this.apiURL + "/GetJobsRespaldos/", {
      responseType: "json", reportProgress: true, observe: "events", headers: new HttpHeaders(
        { 'Content-Type': 'application/json' },
      ),
      params: new HttpParams().set("sucursales", sucursales).set("fecha", fecha).set("tipoConsulta", tipoConsulta)
    });
    //return this.http.get<any>(this.apiURL + "/GetJobsRespaldos/", httpOptions);

  }

  //getFalloResp(sucursales: any, fecha: string): Observable<any> {
  //  const httpOptions = {
  //    headers: new HttpHeaders({
  //      'Content-Type': 'application/json'
  //    }),
  //    params: new HttpParams().set("sucursales", sucursales).set("fecha", fecha)
  //  };
  //  return this.http.get<any>(this.apiURL + "/GetFalloResp/", httpOptions);
  //}

}
