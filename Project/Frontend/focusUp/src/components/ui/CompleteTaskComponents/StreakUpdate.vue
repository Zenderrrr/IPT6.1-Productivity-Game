<script lang="ts" setup>
import PopUpTaskCompletion from '@/components/ui/CompleteTaskComponents/PopUpTaskCompletion.vue'

const props = defineProps<{
  streakCountAfter: number,
  streakCountBefore: number
}>()

const emit = defineEmits<{
  (e: 'submit') : void
}>()

function onSubmit(){
  emit('submit')
}
</script>

<template>
  <PopUpTaskCompletion>
    <div class="shake base-element min-w-[500px]">
      <div class="flex flex-col justify-center items-center">

        <!-- Streak Text-->
        <div class="appear w-full flex justify-center items-center gap-2 text-sm">
          <i class="fa-solid fa-angle-up"></i>
          <span class="uppercase">Streak verlängert!</span>
        </div>

        <!-- Streak icon-->
        <div class="stamp z-1 relative w-[100px] h-[100px] flex justify-center items-center text-[50px] bg-orange-100 border border-b-orange-500 text-orange-500 rounded-full mt-5">
          <div class="absolute w-full h-full ping bg-orange-100 rounded-full z-0"></div>
          <i class="fa-solid fa-fire z-20"></i>
        </div>

        <!-- Streak Count-->
        <div class="appear flex flex-col justify-center items-center">
          <div class="relative h-[90px] w-full">
            <span class="disappear text-[60px] font-bold absolute top-0 right-50% translate-x-[50%]">{{ props.streakCountAfter - 1
              }}</span>
            <span class="streak-count text-[60px] font-bold">{{ props.streakCountAfter }}</span>
          </div>
          <span class="text-sm text-[var(--text-color-light)] font-semibold">Tage in Folge</span>
        </div>

        <div class="appear my-3 flex justify-center items-center gap-2 rounded-full border border-b-green-400 font-semibold text-green-400 px-4 py-2 bg-green-100 text-sm">
          <i class="fa-solid fa-crosshairs"></i>
          <span>{{ props.streakCountAfter }} Tage Streak</span>
        </div>

        <!-- divider-->
        <div class="appear w-full h-0.5 bg-gray-100 my-3"></div>

        <!-- Continue button-->
        <div @click="onSubmit" class="appear scale-animation-sm cursor-pointer rounded-lg w-full px-5 py-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] flex justify-center items-center">
          <button class="cursor-pointer">Weiter</button>
        </div>
      </div>
    </div>
  </PopUpTaskCompletion>
</template>

<style scoped>
  .ping{
    animation: ping 1.5s infinite ease-out;
  }

  @keyframes ping {
    0% {
      transform: scale(1);
      opacity: 1;
    }
    70%{
      transform: scale(1.5);
      opacity: 0;
    }
    100%{
      transform: scale(1.5);
      opacity: 0;
    }
  }

  .appear{
    animation: appear .5s ease-out forwards;
    opacity: 0;
  }

  .appear:nth-child(1){
    animation-delay: .1s;
  }
  .appear:nth-child(2){
    animation-delay: 1s;
  }
  .appear:nth-child(3){
    animation-delay: 2s;
  }
  .appear:nth-child(4){
    animation-delay: 3s;
  }
  .appear:nth-child(5){
    animation-delay: 4s;
  }
  .appear:nth-child(6){
    animation-delay: 4s;
  }

  @keyframes appear {
    0%{
      opacity: 0;
    }
    70%{
      opacity: 1;
    }
    100%{
      opacity: 1;
    }
  }

  .stamp{
    animation: stamp .6s ease-in forwards;
  }

  @keyframes stamp {
    0%{
      opacity: 0;
      transform: scale(1.5);
    }
    50%{
      opacity: 1;
      transform: scale(1.4);
    }
    100%{
      opacity: 1;
      transform: scale(1);
    }
  }

  .shake{
    animation: shake .3s ease-in forwards;
    animation-delay: .62s;
  }

  @keyframes shake {
    0%{
      transform: scale(1) rotate(0deg);
    }
    20%{
      transform: scale(1.05) rotate(2deg);
    }
    40%{
      transform: scale(1) rotate(-2deg);
    }
    60%{
      transform: scale(1.05) rotate(2deg);
    }
    80%{
      transform: scale(1) rotate(-2deg);
    }
    100%{
      transform: scale(1) rotate(0deg);
    }
  }

  .disappear{
    animation: disappear 2s ease-out forwards;
    animation-delay: 3s;
    right: 50%;
  }

  @keyframes disappear {
    0%{
      opacity: 1;
    }
    100%{
      opacity: 0;
      right: 120px;
    }
  }

  .streak-count{
    position: absolute;
    top: 0;
    right: -60px;
    opacity: 0;
    animation: streak 2s linear forwards;
    animation-delay: 3s;
    transform: translateX(50%);
  }

  @keyframes streak {
    0%{
      opacity: 0;
      right: -60px;
    }
    100%{
      opacity: 1;
      right: 50%;
    }
  }
</style>
