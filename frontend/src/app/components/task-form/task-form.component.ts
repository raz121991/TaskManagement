import {
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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

  constructor(private fb: FormBuilder) {
    this.minDate = new Date();
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      dueDate: ['', [Validators.required]],
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


  onSubmit() {
    if (this.taskForm.valid) {
      const task = { ...this.selectedTask, ...this.taskForm.value };

      if (this.selectedTask) {
        this.taskUpdated.emit(task);
      } else {
        this.taskAdded.emit(task);
      }
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
    

    this.selectedTask = null; // Clear the selected task
  };
  
  
}
