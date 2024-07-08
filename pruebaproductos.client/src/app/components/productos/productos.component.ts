/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component } from '@angular/core';
import { ServiceProductos } from '../../services/service.productos';
import { Producto } from '../../models/producto';

@Component({
  selector: 'app-productos',
  templateUrl: './productos.component.html',
  styleUrls: ['./productos.component.css']
})
export class ProductosComponent {
  public productos: Producto[] = [];
  public filtroCodigo: string = '';
  public filtroDescripcion: string = '';
  public filtroFechaCreacion: Date | undefined;
  public seleccionado: number = 0;

  public filtroCategoria: string = '';
  public filtroPrecio: number = 0;

  public filtrosVisible: boolean = true;
  //MODAL
  public titulomodal: string = '';
  public modalBorrar: boolean = false;
  //ERRORES
  public messageError: string | null = null;
  public messageNoResults: string | null = null;
  public url: string = 'https://localhost:7260/';

  constructor(private _serviceProductos: ServiceProductos) { }

  // Limpiar tabla (parrilla)
  limpiarTabla() {
    this.productos = [];
    this.messageNoResults = null;
  }

  // Limpiar los filtros
  limpiarFiltros() {
    this.filtroCodigo = '';
    this.filtroDescripcion = '';
    this.messageError = null;
    this.filtroFechaCreacion= undefined;
    this.filtroCategoria = '';
    this.filtroPrecio = 0;
  }
  //Mostrar y ocultar mensaje de error (codigo tiene que ser un numero)
  showMessage(message: string): void {
    this.messageError = message;
  }
  clearMessage(): void {
    this.messageError = null;
    this.messageNoResults = null;
  }
  //Mostrar TODOS los productos al no proporcionar ningun filtro
  getProductos(): void {
    this._serviceProductos.getAllProducts().subscribe(
      (data: Producto[]) => {
        this.productos = data;
        if (this.productos.length === 0) {
          this.messageNoResults = 'No existen resultados.';
        }
      },
      (error: any) => {
        console.error('Error fetching products', error);
        this.showMessage('Error al obtener productos');
      }
    );
  }

  // Filtrar por campos
  buscarProductos() {
    const params: { [key: string]: any } = {};

    if (this.filtroCodigo) {
      const codigoInt = parseInt(this.filtroCodigo, 10);
      if (!isNaN(codigoInt)) {
        params['codigo'] = codigoInt;
      } else {
        this.showMessage('El código debe ser un número válido.');
        return;
      }
    }

    if (this.filtroDescripcion) {
      params['descripcion'] = this.filtroDescripcion;
    }

    if (this.filtroFechaCreacion instanceof Date) {
      params['fechaCreacion'] = this.filtroFechaCreacion.toISOString();
    }

    if (this.filtroCategoria) {
      params['categoria'] = this.filtroCategoria;
    }

    if (this.filtroPrecio > 0) {
      params['precio'] = this.filtroPrecio;
    }

    this._serviceProductos.llamarApi('', params).subscribe(
      (data: Producto[]) => {
        this.productos = data;
        if (this.productos.length === 0) {
          this.messageNoResults = 'No existen resultados con los filtros proporcionados.';
        }
      },
      (error) => {
        console.error('Error al obtener productos', error);
        this.showMessage('Error al obtener productos con los filtros proporcionados.');
      }
    );

    this.clearMessage();
  }

  //Mostrar y ocultar la ventana de filtros
  toggleFiltros() {
    this.filtrosVisible = !this.filtrosVisible;
  }


  //METODOS PARA EL MODAL

  contenidoDetalles(codigo:number) {
    this.titulomodal = 'Detalles';

  }
  contenidoCrear() {
    this.titulomodal = 'Crear';
  }
  contenidoEditar(codigo: number) {
    this.titulomodal = 'Editar';
  }
  contenidoBorrar(codigo: number) {
    this.seleccionado = codigo;
    this.titulomodal = 'Borrar';
    this.modalBorrar = true;
  }
  borrar() {
    if (!this.seleccionado) {
      console.error('No se ha seleccionado ningún producto para borrar.');
      return;
    }

    this._serviceProductos.deleteProducto(this.seleccionado).subscribe(
      () => {
        // Éxito al eliminar el producto
        this.modalBorrar = false; // Oculta el modal de confirmación
        this.getProductos(); // Actualiza la lista de productos después de la eliminación
      },
      (error) => {
        console.error('Error al borrar producto', error);
        this.showMessage('Error al borrar el producto.');
        this.modalBorrar = false; // Aunque hubo un error, oculta el modal de confirmación
      }
    );
  }


  toggleVariable(variable: boolean) {
    variable = !variable;
  }

}
