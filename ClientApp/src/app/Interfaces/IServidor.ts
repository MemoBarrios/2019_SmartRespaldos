import { ISucursal } from "../home/ISucursal";
import { IRutaRespaldo } from "./IRutaRespaldo";

export interface IServidor
{
  id: number;
  nombre: string;
  ip: string;
  sucursal: ISucursal;
  rutasRespaldos: Array<IRutaRespaldo>;
}
