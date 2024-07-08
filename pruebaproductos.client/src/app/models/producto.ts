export class Producto {
  constructor(
    public codigoProducto: number,
    public descripcionProducto: string,
    public fechaCreacion: Date,
    public categoria: string,
    public precio: number
  ) { }
}
