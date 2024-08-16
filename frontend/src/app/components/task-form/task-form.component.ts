import {
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { Task } from '../../models/task.model';
@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css'],
})
export class TaskFormComponent {
  taskForm: FormGroup;
  minDate: Date;
  @Output() taskAdded = new EventEmitter<Task>();
  @Input() selectedTask: Task | null = null;
  @Output() taskUpdated = new EventEmitter<Task>();
  showTitleError = false;
  showDescriptionError = false;
  showDueDateError = false;

  constructor(private fb: FormBuilder) {
    this.minDate = new Date();
    this.taskForm = this.fb.group({
      title: [''],
      description: [''],
      dueDate: [''],
    });
  }

  
  setTask(task: Task | null): void {
    this.selectedTask = task;
    if (task) {
      this.taskForm.patchValue({
        title: task.title,
        description: task.description,
        dueDate: task.dueDate
      });
    } else {
      this.resetForm();
    }
  }


  onSubmit(formDirective: FormGroupDirective) {
    

      this.showTitleError = !this.taskForm.get('title')?.value ? true : false;
      this.showDescriptionError = !this.taskForm.get('description')?.value ? true : false;
      this.showDueDateError = !this.taskForm.get('dueDate')?.value ? true : false;

      if (this.showTitleError) {
        this.taskForm.get('title')?.setErrors({ invalid: true });
      }
      if (this.showDescriptionError) {
        this.taskForm.get('description')?.setErrors({ invalid: true });
      }
      if (this.showDueDateError) {
        this.taskForm.get('dueDate')?.setErrors({ invalid: true });
      }


      if (this.showTitleError || this.showDescriptionError || this.showDueDateError) {
        return;
      }

      const task = { ...this.selectedTask, ...this.taskForm.value };

      if (this.selectedTask) {
        this.taskUpdated.emit(task);
      } else {
        this.taskAdded.emit(task);
      }
    
      this.resetForm();

   

  }

  resetForm(): void {
      
    this.taskForm.reset({
      title: '',
      description: '',
      dueDate: '',
    });

   
    this.taskForm.markAsPristine();
    this.taskForm.markAsUntouched();
    

    this.selectedTask = null; 
  };

 
  
  
}
