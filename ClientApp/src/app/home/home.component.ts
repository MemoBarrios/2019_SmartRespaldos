import { Component, OnInit } from '@angular/core';
import { IRespaldo } from './IRespaldo';
import { ISucursal } from './ISucursal';
import { RespaldosService } from '../services/respaldos.service';
import { FormBuilder, FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { NgbDateStruct, NgbCalendar, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { DateParserService } from '../services/date-parser.service';
import { HttpEventType, HttpEvent } from '@angular/common/http';
import { IRespaldos } from './IRespaldos';
import { IServidor } from '../Interfaces/IServidor';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  dtRespaldo: Array<IRespaldo>;
  dtRespaldos: Array<IRespaldo[]>;
  dtRutasResp: Array<IServidor>;
  dtSucursales: Array<ISucursal>;
  dtSucResult: Array<ISucursal>;
  dtRevision: any;
  model2: NgbDateStruct;
  fechaPicker: string;
  dtServSelected: Array<ISucursal> = new Array<ISucursal>();
  dtSucs: Array<number>;
  bTodos: boolean = false;
  btnTodos: string = "Marcar Todos";

  constructor(private service: RespaldosService, private formBuilder: FormBuilder, private serviceDate: DateParserService, private fechaAdapter: NgbDateAdapter<string>, private ngbCalendar: NgbCalendar) { }
  
  ngOnInit() {
    this.ConsultaSucursales();
    this.dtRespaldos = new Array<IRespaldo[]>();
    this.fechaPicker = this.fechaAdapter.toModel(this.ngbCalendar.getToday());
  }

  //FUNCION PARA CONSULTAR LOS DATOS DE LOS SERVIDORES DE LAS SUCURSALES ACTIVAS
  ConsultaSucursales() {
    this.dtRespaldos = [];
    this.service.getAllSucursales().subscribe(sucursalesDesdeWS => {
      this.dtSucursales = sucursalesDesdeWS
    }, error => console.error(error));
  }

  //FUNCION PARA CONSULTAR LAS RUTAS DONDE SE VAN A GENERAR LOS RESPALDOS
  ConsultaResp(ip: string, indice?: number) {
    if (indice != null && indice != undefined) {
      this.service.getAllRespaldos(ip).subscribe(respaldosDesdeWS => {
        this.dtRespaldo = respaldosDesdeWS,
          this.dtRespaldos[indice] = this.dtRespaldo;
          //this.dtRespaldos.push(this.dtRespaldo)
          console.log(this.dtRespaldos);
      }, error => console.error(error));

    }
    else {
      this.service.getAllRespaldos(ip).subscribe(respaldosDesdeWS => { this.dtRespaldo = respaldosDesdeWS }, error => console.error(error));
    }
  }
 
  consultar() {
    if (this.dtServSelected.length > 0) {
      for (var i = 0; i < this.dtServSelected.length; i++) {
        this.ConsultaResp(this.dtServSelected[i].ip.toString(), i);
      }
    }
  }

  getRutasResp() {
    this.dtSucs = new Array<number>();
    for (var i = 0; i < this.dtServSelected.length; i++) {
      this.dtSucs.push(this.dtServSelected[i].clave);
    }

    this.service.getRutasResp(JSON.stringify(this.dtSucs)).subscribe((event: HttpEvent<Array<IServidor>>) => {
      switch (event.type) {
        case HttpEventType.Sent:
          console.log('Request sent!');
          document.getElementById("load-pacman").style.display = 'flex';
          window.scrollTo(0, document.body.scrollHeight);
          break;
        case HttpEventType.ResponseHeader:
          console.log('Response header received!');
          break;
        case HttpEventType.DownloadProgress:
          const kbLoaded = Math.round(event.loaded / 1024);
          console.log(`Download in progress! ${kbLoaded}Kb loaded`);
          break;
        case HttpEventType.Response:
          document.getElementById("load-pacman").style.display = 'none';
          console.log('😺 Done!', event.body);
          this.dtRutasResp = event.body;          
      }
    }, error => console.log(error));
  }

  //FUNCION PARA CONSULTAR LOS JOBS DE RESPALDO SEGUN EL TIPO DE CONSULTA: 1.-CONSULTA = SOLO CONSULTAR, 2.-REVISION = REVISION DIARIA DE JOBS
  consultaJobs(tipoConsulta: string) {
    this.dtRevision = "";
    //let fechaSelect: string = this.serviceDate.format(this.fechaPicker);
    this.dtSucs = new Array<number>();

    for (var i = 0; i < this.dtServSelected.length; i++) {
      this.dtSucs.push(this.dtServSelected[i].clave);
    }
    this.service.getJobsRespaldos(JSON.stringify(this.dtSucs), this.fechaPicker, tipoConsulta).subscribe((event: HttpEvent<any>) => {
      switch (event.type) {
        case HttpEventType.Sent:
          console.log('Request sent!');
          document.getElementById("load-pacman").style.display = 'flex';
          window.scrollTo(0, document.body.scrollHeight);
          break;
        case HttpEventType.ResponseHeader:
          console.log('Response header received!');
          break;
        case HttpEventType.DownloadProgress:
          const kbLoaded = Math.round(event.loaded / 1024);
          console.log(`Download in progress! ${kbLoaded}Kb loaded`);
          break;
        case HttpEventType.Response:
          document.getElementById("load-pacman").style.display = 'none';
          console.log('😺 Done!', this.dtRevision=event.body);
      }
    },error => console.log(error));       
  }

  Haber() {
    console.log(this.dtRutasResp[1].rutasRespaldos[1]);
  }

  chkSelect(sucSelected: ISucursal) {
    if (sucSelected.seleccionado == false) {
      sucSelected.seleccionado = false;
      for (var i = 0; i < this.dtServSelected.length; i++) {
        if (this.dtServSelected[i].clave == sucSelected.clave) {
          this.dtServSelected.splice(i, 1);
        }
      }
      console.log(this.dtServSelected);
    }
    else {
      sucSelected.seleccionado = true;
      this.dtServSelected.push(sucSelected);
      console.log(this.dtServSelected);
    }

   
  }

  //METODO PARA SELECCIONAR Y QUITAR SELECCION DE LOS CHECKBOX
  selectAll() {
    if (this.bTodos == true) {
      for (var i = 0; i < this.dtSucursales.length; i++) {
        this.dtSucursales[i].seleccionado = false;
      }
      if (this.dtServSelected.length != 0) {
        this.dtServSelected = [];
      }
      this.bTodos = false;
      this.btnTodos = "Marcar Todos";
    }
    else {
      for (var i = 0; i < this.dtSucursales.length; i++) {
        this.dtSucursales[i].seleccionado = true;
      }
      if (this.dtServSelected.length != 0) {
        this.dtServSelected = [];
      }
      this.dtServSelected = this.dtSucursales.slice();
      this.bTodos = true;
      this.btnTodos = "Desmarcar Todos";
    }
  }
  
}
