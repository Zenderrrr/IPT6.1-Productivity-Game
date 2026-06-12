<script lang="ts" setup>
  const emit = defineEmits<{
    (e: 'cancel') : void
    (e: 'submit') : void
  }>();

  const props = defineProps<{
    title: string,
  }>()

  function cancel() {
    emit('cancel');
  }
</script>

<template>
    <div class="fixed inset-0 flex justify-center items-center backdrop-blur-xs z-999 px-4 py-6">
      <div class="base-element w-full max-w-[34rem] max-h-[90vh] overflow-y-auto">
        <!-- Title and close-->
        <div class="flex items-center justify-between gap-4 w-full">
          <span class="font-semibold text-lg sm:text-xl break-words">{{ props.title }}</span>
          <button @click="cancel" class="shrink-0 hover:border-[var(--primary-color)] hover:text-[var(--primary-color)] duration-200 transition border-1 border-transparent flex justify-center items-center bg-gray-100 text-gray-400 w-[35px] h-[35px] rounded-lg text-sm cursor-pointer">
            <i class="fa-solid fa-x"></i>
          </button>
        </div>

        <slot></slot>
      </div>
    </div>
</template>