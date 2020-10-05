import { Component, Inject, OnInit } from '@angular/core';
import { KeyValuePipe } from '@angular/common/';
import { IRespaldo } from './IRespaldo';
import { ISucursal } from './ISucursal';
import { RespaldosService } from '../services/respaldos.service';
import { FormBuilder, FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { IRespaldos } from './IRespaldos';
import { error } from 'protractor';
import { NgbDateStruct, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { DateParserService } from '../services/date-parser.service';
import { ISucursalDTO } from './ISucursalDTO';
import { HttpEventType, HttpEvent } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  dtRespaldo: Array<IRespaldo>;
  dtRespaldos: Array<IRespaldo[]>;
  dtSucursales: Array<ISucursal>;
  dtSucResult: Array<ISucursal>;
  dtRevision: any;
  fecha: Date;
  model2: NgbDateStruct;
  //dtSucursales: any = [{ clave: 201, nombre: '201', ip: '', seleccionado: false },
  //  { clave: 202, nombre: '202', ip: '', seleccionado: false  },
  //  { clave: 203, nombre: '203', ip: '', seleccionado: false  },
  //  { clave: 204, nombre: '204', ip: '', seleccionado: false  },
  //  { clave: 205, nombre: '205', ip: '', seleccionado: false  }];
  dtServSelected: Array<ISucursal> = new Array<ISucursal>();
  dtSucs: Array<number>;
  bTodos: boolean = false;
  btnTodos: string = "Marcar Todos";

  constructor(private service: RespaldosService, private formBuilder: FormBuilder, private serviceDate: DateParserService, private ngbCalendar: NgbCalendar) { }
  
  ngOnInit() {
    this.ConsultaSucursales();
    this.dtRespaldos = new Array<IRespaldo[]>();
    this.model2 = this.ngbCalendar.getToday();
  }

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

  //ConsultaRespSucursales(sucursales: Array<ISucursal>) {
  //  console.log(sucursales);
  //  this.service.getRespaldoSucursales(sucursales).subscribe(respaldosDesdeWS => {
  //    this.dtSucResult = respaldosDesdeWS        
  //  });
  //}

  ConsultaSucursales() {
    this.dtRespaldos = [];
    this.service.getAllSucursales().subscribe(sucursalesDesdeWS => {
    this.dtSucursales = sucursalesDesdeWS
    }, error => console.error(error));
  }

  consultar() {
    if (this.dtServSelected.length > 0) {
      for (var i = 0; i < this.dtServSelected.length; i++) {
        this.ConsultaResp(this.dtServSelected[i].ip.toString(), i);
      }
    }
  }

  consultaJobs(tipoConsulta: string) {
    //this.bTodos = false;
    //this.selectAll();
    console.log(tipoConsulta);
    this.dtRevision = "";
    let fechaSelect: string = this.serviceDate.format(this.model2);
    this.dtSucs = new Array<number>();

    for (var i = 0; i < this.dtServSelected.length; i++) {
      this.dtSucs.push(this.dtServSelected[i].clave);
    }
    this.service.getJobsRespaldos(JSON.stringify(this.dtSucs), fechaSelect, tipoConsulta).subscribe((event: HttpEvent<any>) => {
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

  Haber(tipo) {
    console.log(tipo);
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
