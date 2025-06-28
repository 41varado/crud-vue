<template>
    <div class="card flex justify-center">
        <Toast />
        <Dialog v-model:visible="dialogVisible" modal header="Register Accountant" :style="{ width: '25rem'}">
            <div class="flex items-center gap-4 mb-4">
                <label for="name" class="font-semibold w-24">Name</label>
                <InputText v-model="name" required id="name" class="flex-auto" autocomplete="off"></InputText>
            </div>
            <div class="flex items-center gap-4 mb-4">
                <label for="birthDate" class="font-semibold w-24">Birth date</label>
                <DatePicker v-model="birthDate" name="birthDate" fluid/>
            </div>
            <div class="flex justify-end gap-4">
                <Button label="Save" @click="saveAccountant" />
            </div>
        </Dialog>
    </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, mergeProps } from 'vue';
  import type { Accounting } from '@/models/Accounting';
  import Dialog from 'primevue/dialog';
  import Button from 'primevue/button';
  import DatePicker from 'primevue/datepicker';
  import InputText from 'primevue/inputtext';
  import { useToast } from 'primevue/usetoast';
  import Toast from 'primevue/toast';
import axios from 'axios';

  const toast = useToast();
  const name = ref<string>('');
  const birthDate = ref<Date | null>(null);
  const dialogVisible = ref<boolean>(true);

  const emit = defineEmits(['accountantSaved']);

    const saveAccountant = () => {
        if (name.value && birthDate.value) {
            const newAccountant: Accounting = {
                id: "",
                name: name.value,
                birthDate: birthDate.value.toISOString(),
            };
            
            axios.post('https://localhost:44349/api/v1/accounting', {
                name: newAccountant.name,
                birthDate: newAccountant.birthDate
            })
            .then((response) => {
                console.log(response);
                toast.add({
                    severity: 'success',
                    summary: 'Success',
                    detail: 'Accountant saved successfully!',
                    life: 3000
                });
                emit('accountantSaved');
            })
            .catch((error) => {
                console.error('Error saving accountant:', error);
                toast.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error.message,
                    life: 3000
                });
            })
        } else {
            toast.add({
                severity: 'error',
                summary: 'Error',
                detail: 'Please fill in all fields.',
                life: 3000
            })
        }
        dialogVisible.value = false;
    };
</script>