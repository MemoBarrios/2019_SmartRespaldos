import { Injectable } from '@angular/core';
import { NgbDateAdapter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class FechaAdapterService extends NgbDateAdapter<string> {

  readonly DELIMITER = '/';

  fromModel(value: string): NgbDateStruct | null {
    if (value) {
      var valor = value.toString();
      let date = valor.split(this.DELIMITER);
      return {
        day: parseInt(date[0], 10),
        month: parseInt(date[1], 10),
        year: parseInt(date[2], 10)
      };
    }
    return null;
  }

  toModel(date: NgbDateStruct | null): string | null {
    return date ? date.day + this.DELIMITER + date.month + this.DELIMITER + date.year : null;
  }
}
