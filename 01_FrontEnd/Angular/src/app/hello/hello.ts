import { Component } from '@angular/core';

@Component({
  selector: 'app-hello',
  imports: [],
  templateUrl: './hello.html',
  styleUrl: './hello.scss',
})
export class Hello {
  protected title = 'Hello, my-finances-app';
  protected isDisabled = false;

  onClick(): void {
    console.log('Button clicked!');
    this.isDisabled = this.isDisabled ? false : true;
  }
}
