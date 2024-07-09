import { Directive, ElementRef, HostListener, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[appDraggable]'
})

export class DraggableDirective implements AfterViewInit {
  private modalElement!: HTMLElement;
  private topStart!: number;
  private leftStart!: number;
  private isDraggable!: boolean;
  private handleElement!: HTMLElement;

  constructor(public element: ElementRef) { }

  ngAfterViewInit() {
    // Se obtiene el elemento modal
    let element = this.element.nativeElement;
    this.handleElement = this.element.nativeElement;

    // Se le asigna el cursor pointer al elemento que se va a mover
    this.handleElement.style.cursor = 'pointer';

    // Se busca el elemento padre (modal-content)
    element = element.closest('.modal-content');

    // Se asigna el elemento modal
    this.modalElement = element;

    this.modalElement.style.position = 'relative';
  }

  @HostListener('mousedown', ['$event'])
  public onMouseDown(event: MouseEvent) {
    // Si se hace click derecho o no se ha inicializado el handleElement
    if (event.button === 2 || !this.handleElement) {
      return;
    }

    // Si el elemento que se intenta mover no es el handleElement
    // o no es un hijo del handleElement
    if (event.target !== this.handleElement && !this.searchParentNode(<any>event.target, this.handleElement)) {
      return;
    }

    this.isDraggable = true;

    // Guarda la posición inicial del modal
    this.topStart = event.clientY - Number(this.modalElement.style.top.replace('px', ''));
    this.leftStart = event.clientX - Number(this.modalElement.style.left.replace('px', ''));
    event.preventDefault();
  }

  // Al soltar el modal, se desactiva el movimiento
  @HostListener('document:mouseup', ['$event'])
  onMouseUp(event: MouseEvent) {
    this.isDraggable = false;
  }

  // Al mover el modal, se cambia la posición
  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    if (this.isDraggable) {
      this.modalElement.style.top = (event.clientY - this.topStart) + 'px';
      this.modalElement.style.left = (event.clientX - this.leftStart) + 'px';
    }
  }

  // Al salir del modal, se desactiva el movimiento
  @HostListener('document:mouseleave', ['$event'])
  onMouseLeave(event: MouseEvent) {
    this.isDraggable = false;
  }

  // Resetea la posición del modal
  resetPosition() {
    this.modalElement.style.top = '';
    this.modalElement.style.left = '';
  }

  // Busca el nodo padre
  private searchParentNode(element: Node, tag: Node): Node | null {
    while (element.parentNode) {
      element = element.parentNode;
      if (element === tag) {
        return element;
      }
    }
    return null;
  }
}
