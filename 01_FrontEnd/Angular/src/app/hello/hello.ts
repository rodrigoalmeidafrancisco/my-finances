import { Component, computed, effect, signal } from '@angular/core';

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

  protected count = signal(0);
  protected doubleCount = computed(() => {   return this.count() * 2;  });
  private readonly countLog = effect(() => { console.log('Count changed:', this.count()); });

  // protected getDoubleCount(): number {
  //   console.log('getDoubleCount called');
  //   return this.count() * 2;
  // }

  protected increment(): void { this.count.update(item => item + 1);}
  protected decrement(): void { this.count.update(item => item - 1);}
  protected reset(): void { this.count.set(0);}

}
