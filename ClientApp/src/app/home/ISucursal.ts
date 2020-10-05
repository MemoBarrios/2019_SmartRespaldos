import { IRespaldo } from "./IRespaldo";

export interface ISucursal {
  clave: number;
  nombre: string;
  ip: string;
  db: string;
  seleccionado: boolean;
  respaldos: IRespaldo[];
}
